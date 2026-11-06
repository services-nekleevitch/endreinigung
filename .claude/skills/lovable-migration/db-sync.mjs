#!/usr/bin/env node
// Compare Lovable's live Supabase tables with what we last synced into EndReinigung.
//
// Lovable keeps some content ONLY in its database (edited in the Lovable UI, no migration file):
//   - ratgeber_articles  -> EndReinigung/App_Data/ratgeber-articles.json   (applied by hand/agent)
//   - google_reviews     -> EndReinigung/App_Data/google-reviews.json      (applied by --apply-reviews)
//
// State of the last sync lives in .lovable-db-sync.json at the repo root: one content
// fingerprint per row. The script reports rows that are new / changed / removed since then.
//
// Usage (from the repo root):
//   node .claude/skills/lovable-migration/db-sync.mjs                 # report only
//   node .claude/skills/lovable-migration/db-sync.mjs --apply-reviews # also rewrite google-reviews.json
//   node .claude/skills/lovable-migration/db-sync.mjs --write-state   # record current live state as synced
//
// Run --write-state only AFTER the changes it reported have been applied to our JSON files.

import fs from "node:fs";
import path from "node:path";
import crypto from "node:crypto";

const ROOT = process.cwd();
const STATE_FILE = path.join(ROOT, ".lovable-db-sync.json");
const ENV_FILE = path.join(ROOT, "rich-lease-shine", ".env");
const REVIEWS_FILE = path.join(ROOT, "EndReinigung", "App_Data", "google-reviews.json");
const PLACE_ID = "ChIJC48RWyzclEsRsHndo70t9eU";

// Only fields that represent content. Volatile fields are left out on purpose:
// google_reviews.updated_at and relative_time_description change on every daily Google refresh.
const TABLES = {
  ratgeber_articles: {
    query: "select=*&order=sort_order",
    key: (r) => r.slug,
    fields: ["slug", "title_de", "title_en", "intro_de", "intro_en", "content_de", "content_en",
             "cover_image", "is_published", "category_id", "sort_order"],
    target: "EndReinigung/App_Data/ratgeber-articles.json",
  },
  google_reviews: {
    query: `place_id=eq.${PLACE_ID}&select=*&order=review_time.desc`,
    key: (r) => r.id,
    fields: ["id", "author_name", "rating", "text", "review_time", "profile_photo_url"],
    target: "EndReinigung/App_Data/google-reviews.json",
  },
  google_reviews_cache: {
    query: `place_id=eq.${PLACE_ID}&select=*`,
    key: (r) => r.place_id,
    fields: ["place_id", "rating", "user_ratings_total"],
    target: "EndReinigung/App_Data/google-reviews.json (rating)",
  },
};

const args = new Set(process.argv.slice(2));

function readEnv() {
  const env = {};
  for (const line of fs.readFileSync(ENV_FILE, "utf8").split(/\r?\n/)) {
    const m = line.match(/^\s*([A-Z0-9_]+)\s*=\s*"?([^"]*)"?\s*$/);
    if (m) env[m[1]] = m[2];
  }
  return env;
}

const fingerprint = (row, fields) =>
  crypto.createHash("sha256")
    .update(JSON.stringify(fields.map((f) => row[f] ?? null)))
    .digest("hex").slice(0, 16);

async function fetchTable(env, name, query) {
  const url = `${env.VITE_SUPABASE_URL}/rest/v1/${name}?${query}`;
  const key = env.VITE_SUPABASE_PUBLISHABLE_KEY;
  const res = await fetch(url, { headers: { apikey: key, Authorization: `Bearer ${key}` } });
  if (!res.ok) throw new Error(`${name}: HTTP ${res.status} ${await res.text()}`);
  return res.json();
}

const env = readEnv();
const previous = fs.existsSync(STATE_FILE) ? JSON.parse(fs.readFileSync(STATE_FILE, "utf8")) : { tables: {} };
const next = { syncedAt: new Date().toISOString().slice(0, 10), tables: {} };
const live = {};
let anyChange = false;

for (const [name, cfg] of Object.entries(TABLES)) {
  const rows = await fetchTable(env, name, cfg.query);
  live[name] = rows;
  const rowsState = Object.fromEntries(rows.map((r) => [cfg.key(r), fingerprint(r, cfg.fields)]));
  next.tables[name] = { rowCount: rows.length, rows: rowsState };

  const before = previous.tables?.[name]?.rows ?? {};
  const added = Object.keys(rowsState).filter((k) => !(k in before));
  const removed = Object.keys(before).filter((k) => !(k in rowsState));
  const changed = Object.keys(rowsState).filter((k) => k in before && before[k] !== rowsState[k]);

  console.log(`\n== ${name}  (${rows.length} rows live, ${Object.keys(before).length} at last sync) -> ${cfg.target}`);
  if (!previous.tables?.[name]) console.log("   no previous state recorded: treat every row as needing a check");
  for (const k of added) console.log(`   + new      ${k}`);
  for (const k of changed) console.log(`   ~ changed  ${k}`);
  for (const k of removed) console.log(`   - removed  ${k}`);
  if (!added.length && !changed.length && !removed.length && previous.tables?.[name]) console.log("   unchanged");
  if (added.length || changed.length || removed.length || !previous.tables?.[name]) anyChange = true;
}

if (args.has("--apply-reviews")) {
  const rating = Number(live.google_reviews_cache[0]?.rating ?? 5);
  const out = {
    placeId: PLACE_ID,
    rating,
    snapshotDate: next.syncedAt,
    reviews: live.google_reviews.map((r) => ({
      id: r.id, authorName: r.author_name, rating: r.rating, text: r.text,
      reviewTime: r.review_time, profilePhotoUrl: r.profile_photo_url || null,
    })),
  };
  fs.writeFileSync(REVIEWS_FILE, JSON.stringify(out, null, 2).replace(/\n/g, "\r\n") + "\r\n");
  console.log(`\nwrote ${out.reviews.length} reviews to ${path.relative(ROOT, REVIEWS_FILE)}`);
}

if (args.has("--write-state")) {
  fs.writeFileSync(STATE_FILE, JSON.stringify(next, null, 2) + "\n");
  console.log(`\nrecorded live state in ${path.relative(ROOT, STATE_FILE)}`);
} else if (anyChange) {
  console.log("\nApply the changes above, then re-run with --write-state to mark them synced.");
}
