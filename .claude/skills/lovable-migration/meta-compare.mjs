#!/usr/bin/env node
// Compare <title> and meta description of every sitemap URL between our site and Lovable's
// CURRENT code, then use git history to say which side changed the text last.
//
// Prerequisites (see SKILL.md, step 6b):
//   - our site running        (default http://localhost:5031, e.g. `dotnet watch` / Ctrl+F6)
//   - Lovable running locally (default http://127.0.0.1:8089, `npx vite --port 8089 --strictPort --host 127.0.0.1`
//                              in rich-lease-shine). Do NOT use rich-lease-shine.lovable.app:
//                              the published site lags behind the code.
//   - Google Chrome installed (msedge.exe returns empty output when started from node)
//
// Usage (from the repo root):
//   node .claude/skills/lovable-migration/meta-compare.mjs [--ours URL] [--lovable URL] [--out FILE]
//
// Output: one line per differing title/description, classified as
//   LOVABLE NEWER  -> Lovable changed it after our text: migrate it
//   ours newer     -> our text is more recent (e.g. the 2026-05-17 SEO title rewrite): leave it
//   ?              -> could not date one side (DB-driven Ratgeber text, templated Bezirk titles): check by hand
//   ...? (fragment match, verify) -> dated from a leading fragment only: a hint, check by hand

import fs from "node:fs";
import os from "node:os";
import path from "node:path";
import { execFile, execFileSync } from "node:child_process";

const arg = (name, def) => { const i = process.argv.indexOf(name); return i > 0 ? process.argv[i + 1] : def; };
const ROOT = process.cwd();
const OURS = arg("--ours", "http://localhost:5031");
const LOVABLE = arg("--lovable", "http://127.0.0.1:8089");
const OUT = arg("--out", path.join(os.tmpdir(), "lovable-meta-compare.json"));
const CHROME = ["C:/Program Files/Google/Chrome/Application/chrome.exe", "C:/Program Files (x86)/Google/Chrome/Application/chrome.exe",
  "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome", "/usr/bin/google-chrome"].find((p) => fs.existsSync(p));
if (!CHROME) { console.error("Google Chrome not found"); process.exit(1); }

const locs = (file) => [...fs.readFileSync(file, "utf8").matchAll(/<loc>([^<]+)<\/loc>/g)].map((m) => new URL(m[1].trim()).pathname);
const paths = [...new Set([
  ...locs(path.join(ROOT, "EndReinigung/wwwroot/sitemap.xml")),
  ...locs(path.join(ROOT, "rich-lease-shine/public/sitemap.xml")),
])].sort();

const decode = (s) => s
  .replace(/&#x([0-9a-f]+);/gi, (_, h) => String.fromCodePoint(parseInt(h, 16)))
  .replace(/&#(\d+);/g, (_, d) => String.fromCodePoint(parseInt(d, 10)))
  .replace(/&amp;/g, "&").replace(/&quot;/g, '"').replace(/&#39;|&#x27;/g, "'").replace(/&lt;/g, "<").replace(/&gt;/g, ">")
  .replace(/\s+/g, " ").trim();

function extract(html) {
  const title = (html.match(/<title[^>]*>([\s\S]*?)<\/title>/) || [])[1];
  const metas = [...html.matchAll(/<meta\b[^>]*>/g)].map((m) => m[0]).filter((m) => /name=["']description["']/.test(m));
  // Lovable: index.html ships a default description; the page's own one (react-helmet) carries data-rh.
  const pick = metas.find((m) => /data-rh/.test(m)) || metas[metas.length - 1];
  const desc = pick ? (pick.match(/content="([^"]*)"/) || pick.match(/content='([^']*)'/) || [])[1] : undefined;
  return { title: title ? decode(title) : null, desc: desc ? decode(desc) : null, helmet: /data-rh/.test(html) };
}

async function ours(p) {
  try {
    const res = await fetch(OURS + p, { redirect: "manual" });
    if (res.status >= 300 && res.status < 400) return { status: res.status, redirect: res.headers.get("location") };
    return { status: res.status, ...extract(await res.text()) };
  } catch (e) { return { status: "ERR " + e.message }; }
}

// Fresh browser profile per page: Lovable stores the language in localStorage ("preferred-language"),
// so after an /en/ page a reused profile renders every later page in English.
function render(p, budget) {
  const prof = fs.mkdtempSync(path.join(os.tmpdir(), "lovable-meta-"));
  return new Promise((resolve) => {
    execFile(CHROME, ["--headless=new", "--disable-gpu", "--no-first-run", "--lang=de-CH", `--user-data-dir=${prof}`,
      `--virtual-time-budget=${budget}`, "--dump-dom", LOVABLE + p],
      { maxBuffer: 50 * 1024 * 1024, timeout: 120000 }, (err, stdout) => {
        try { fs.rmSync(prof, { recursive: true, force: true }); } catch {}
        resolve(err && !stdout ? { title: null, desc: null, err: String(err.message).slice(0, 80) } : extract(stdout));
      });
  });
}
async function lovable(p) {
  let r = await render(p, 15000);
  if (r.err || !r.helmet) r = await render(p, 30000); // vite compiles routes on demand: first hit can be slow
  return r;
}
async function pool(items, n, fn) {
  const out = new Array(items.length); let i = 0;
  await Promise.all(Array.from({ length: n }, async () => { while (i < items.length) { const k = i++; out[k] = await fn(items[k]); } }));
  return out;
}

// When did each side last introduce this exact text? (git pickaxe; falls back to a leading fragment)
function lastAdded(cwd, text, scopes) {
  if (!text) return null;
  const variants = [text, text.split(" | ")[0], text.split(" — ")[0], text.slice(0, 45)]
    .map((s) => s.trim()).filter((s, i, a) => s.length >= 12 && a.indexOf(s) === i);
  for (const v of variants)
    for (const needle of [v, v.replace(/&/g, "&amp;")]) {
      const out = execFileSync("git", ["log", "-1", "--format=%ad", "--date=short", "-S", needle, "--", ...scopes], { cwd, encoding: "utf8" }).trim();
      if (out) return { date: out, partial: v !== text };
    }
  return null;
}

await render("/", 20000); // warm up
const results = await pool(paths, 4, async (p) => ({ path: p, ours: await ours(p), lovable: await lovable(p) }));
for (const r of results) if (r.lovable.err || !r.lovable.helmet) r.lovable = await lovable(r.path); // sequential retry
fs.writeFileSync(OUT, JSON.stringify(results, null, 2));

let same = 0; const rows = [];
for (const r of results) {
  if (r.ours.redirect) { rows.push(`${r.path}\n   ours redirects ${r.ours.status} -> ${r.ours.redirect}`); continue; }
  if (r.lovable.err || !r.lovable.helmet) { rows.push(`${r.path}\n   Lovable has no page meta here (route removed, or render failed)`); continue; }
  if (r.ours.status !== 200) { rows.push(`${r.path}\n   ours HTTP ${r.ours.status}`); continue; }
  let differs = false;
  for (const f of ["title", "desc"]) {
    const o = r.ours[f], l = r.lovable[f];
    if (o === l) continue;
    differs = true;
    const ld = lastAdded(path.join(ROOT, "rich-lease-shine"), l, ["src", "supabase"]);
    const od = lastAdded(ROOT, o, ["EndReinigung"]);
    // A fragment match can hit unrelated text (e.g. an article title inside a links list), so it only hints.
    const verdict = !ld || !od ? "?" : (ld.date > od.date ? "LOVABLE NEWER" : "ours newer") + (ld.partial || od.partial ? "? (fragment match, verify)" : "");
    const d = (x) => x ? x.date + (x.partial ? "~" : "") : "n/a";
    rows.push(`${r.path}  [${f}]  ${verdict}  (lovable ${d(ld)}, ours ${d(od)})\n   ours:    ${o}\n   lovable: ${l}`);
  }
  if (!differs) same++;
}
console.log(rows.join("\n\n"));
console.log(`\n${results.length} URLs, ${same} identical. Raw results: ${OUT}`);
console.log("Dates: last commit that added the exact text (~ = only a leading fragment matched).");
