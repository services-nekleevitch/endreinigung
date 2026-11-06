---
name: lovable-migration
description: Migrate changes from the Lovable React repo (rich-lease-shine) into this ASP.NET Razor project. Use when the user asks to "migrate from Lovable", "pull Lovable changes", "update the website with new Lovable commits", or similar.
---

# Lovable → ASP.NET Migration

This project mirrors a Lovable-generated React site (`rich-lease-shine`) as an SEO-optimized ASP.NET Core Razor site. Changes made in Lovable are pushed to its own GitHub repo, and must be manually translated to Razor views here.

## File locations

- **Sync tracker**: [`.lovable-sync`](../../../.lovable-sync) — baseline commit hash, Lovable repo URL
- **Lovable clone** (may not exist yet): `rich-lease-shine/` at the repo root — clone target for the Lovable GitHub repo
- **ASP.NET project**: [`EndReinigung/`](../../../EndReinigung/)
  - Controller: [`EndReinigung/Controllers/HomeController.cs`](../../../EndReinigung/Controllers/HomeController.cs)
  - Views: [`EndReinigung/Views/Home/`](../../../EndReinigung/Views/Home/) (one `.cshtml` per page)
  - Partials: `_HeroH1`, `_Footer`, `_FixedPricing`, `_VideoSection`, `_LP2FAQ`, `_LetzteEndreinigungen`, `_Var2Services`, `_EndreinigungInfo`, etc.
  - Layout: [`EndReinigung/Views/Shared/_Layout.cshtml`](../../../EndReinigung/Views/Shared/_Layout.cshtml) (supports `ViewData["Title"]`, `["Description"]`, `["Keywords"]`, `["Canonical"]`)
  - Static files: [`EndReinigung/wwwroot/`](../../../EndReinigung/wwwroot/) (images in `img/`, sitemap, robots.txt)

> **Note**: There are two projects. `EndReinigung/` is the active one we migrate into. `EndreinigungZurich/` is an older shell project — leave it alone.

## Workflow

### 1. Get the current Lovable version

**Always pull first** — even if the clone exists, it may be stale. The whole migration is wrong if you diff against an outdated HEAD.

```bash
cd rich-lease-shine && git pull
```

If the clone doesn't exist yet (repo is private → use PAT or `gh repo clone`):

```bash
git clone https://TOKEN@github.com/services-nekleevitch/rich-lease-shine.git
```

After pulling, confirm the new HEAD is ahead of the baseline:

```bash
cd rich-lease-shine
BASELINE=$(grep last_migrated_commit ../.lovable-sync | cut -d= -f2)
git log $BASELINE..HEAD --oneline | wc -l    # should be > 0; if 0, nothing to migrate
```

### 2. Read the baseline and show the delta

The baseline commit hash lives in `.lovable-sync` as `last_migrated_commit=<hash>`.

```bash
cd rich-lease-shine
BASELINE=$(grep last_migrated_commit ../.lovable-sync | cut -d= -f2)
git log $BASELINE..HEAD --oneline                                    # list all commits
git diff $BASELINE..HEAD --name-status -- src/ public/               # added/deleted/modified files
git diff $BASELINE..HEAD --stat -- src/ public/ | tail -30           # line counts
```

Skim the commit messages — Lovable commits are noisy ("Changes", "Work in progress", "Save plan"), so the name-status and stat outputs are where the signal is.

### 2b. Check Lovable's database content (not in git!)

Some content lives **only in Lovable's Supabase database**. It is edited in the Lovable UI and never shows up as a commit or a `supabase/migrations/*.sql` file, so the git diff above misses it. (2026-11-06: the `endreinigung-kosten-zuerich` article had been rewritten from 3 to 41 sections in the DB in May and nobody noticed.)

| Supabase table | Our copy | How to apply |
| --- | --- | --- |
| `ratgeber_articles` | `EndReinigung/App_Data/ratgeber-articles.json` | By hand / agent (see below) |
| `google_reviews` + `google_reviews_cache` | `EndReinigung/App_Data/google-reviews.json` | `--apply-reviews` (mechanical) |

The **sync state** (which rows we last copied) is recorded in [`.lovable-db-sync.json`](../../../.lovable-db-sync.json) at the repo root: one content fingerprint per row. The script [`db-sync.mjs`](db-sync.mjs) compares the live tables against it and lists every row that is new, changed or removed since the last sync:

```bash
node .claude/skills/lovable-migration/db-sync.mjs                  # report only (read-only)
node .claude/skills/lovable-migration/db-sync.mjs --apply-reviews  # also rewrite google-reviews.json
node .claude/skills/lovable-migration/db-sync.mjs --write-state    # mark the current live state as synced
```

It reads the Supabase URL and the public anon key from `rich-lease-shine/.env` — read-only, the same access the Lovable site itself uses. **Never write to Lovable's database.**

Fingerprints only cover content fields. Reviews deliberately ignore `updated_at` and `relative_time_description`, because Lovable's daily Google refresh rewrites those every morning. Otherwise every review would look changed every day.

**Applying article changes** (`ratgeber_articles` → our JSON):

- DB `title_de / intro_de / content_de` → our `titleDe / introDe / blocks`, and `title_en / intro_en / content_en` → `titleEn / introEn / blocksEn`. The block shapes are identical (`text`, `checklist`, `tip`, `table`, `image`, `faq`, `numbered`, `bulleted`, `formula`, `links`). Our renderer `Views/Home/RatgeberArticle.cshtml` supports all of them.
- Image `src` `/ratgeber-images/x.jpg` → our `/img/ratgeber/x.jpg`. Copy the file from `rich-lease-shine/public/ratgeber-images/` if it is missing.
- Keep our `coverImage`: the DB `cover_image` is usually empty, because Lovable imports covers in `Ratgeber.tsx` / `RatgeberArticle.tsx`.
- Also check `RatgeberArticle.tsx` for per-slug `metaOverrides`, `dateOverrides`, `authorBylineOverrides`, and `src/lib/readingTimeOverrides.ts`. Map them to `metaTitleDe`, `metaDescriptionDe`, `dateModified`, `authorBylineDe` and `readingTimeMinutes`.
- Before overwriting a changed article, diff it field by field. Our copy can contain deliberate local fixes, for example internal links rewritten to our URLs (`/umzugsreinigung-stadt-zuerich`). Ask the user when in doubt.
- Edit the JSON with a node script that `JSON.parse` → modify → `JSON.stringify(data, null, 2)` with CRLF line endings. That round-trips byte-identically, so the diff touches only the changed article.

After the changes are applied, run `--write-state` and commit `.lovable-db-sync.json` together with the JSON files.

> `App_Data/*.json` is loaded **once at startup**, from the build output (`bin/Debug/net8.0/App_Data/`). `dotnet watch` does not copy it there or restart. Rebuild, or copy the file into `bin/...` and restart the app (Ctrl+R in the watch terminal) to see the change locally.

### 3. Categorize the changes

Group into buckets before editing anything:

- **Small text / SEO tweaks** — hero benefit copy, meta keywords, title changes, email domain. Apply in bulk with Edit.
- **Footer / Navigation changes** — new links, URL rewrites. Usually [`_Footer.cshtml`](../../../EndReinigung/Views/Shared/_Footer.cshtml).
- **New pages** — a new `.tsx` in `src/pages/` → new `.cshtml` + new controller action + sitemap entry.
- **New routes / redirects** — check [`src/App.tsx`](../../../rich-lease-shine/src/App.tsx) `<Route>` entries. Each needs a controller action (view route) or `RedirectPermanent` (redirect route).
- **Major rewrites** — big diffs on a single page (e.g. Praxisreinigung +800 lines). Delegate to a subagent (`sonnet` model), one agent per page.
- **New assets** — images added to `src/assets/`. Copy matching files into `EndReinigung/wwwroot/img/`.
- **Sitemap / robots.txt** — [`public/sitemap.xml`](../../../rich-lease-shine/public/sitemap.xml) changes mirror to [`EndReinigung/wwwroot/sitemap.xml`](../../../EndReinigung/wwwroot/sitemap.xml).
- **Deleted pages** — pages removed from React (`Var1/2/3`, `LP2`). Leave the Razor view in place unless user confirms removal — deletion is irreversible and the Razor view may still be accessed elsewhere.

### 4. Conversion rules (React → Razor)

| React | Razor equivalent |
| --- | --- |
| `useLanguage()` / `t("key")` | `@SharedLocalizer["key"]` if key exists, else hardcode German |
| `Helmet` → `<title>`, `<meta>` | `ViewData["Title"]`, `ViewData["Description"]`, `ViewData["Keywords"]`, `ViewData["Canonical"]` |
| `<FAQSchema faqs={...} />` | `@section Head { <script type="application/ld+json">...</script> }` with `@@` escaping for `@context`/`@type` |
| Lucide `<HeartPulse />` etc. | Inline `<svg>` (copy viewBox + path from lucide.dev) |
| `<Link to="/x">` | `<a href="/x">` |
| `<Button>` | `<button>` / `<a>` with Tailwind classes |
| `<Card>` | `<div class="rounded-lg border bg-card shadow-sm">` |
| `useState` / interactivity | Inline `<script>` or `<details>`/`<summary>` for accordions |
| Supabase mutations | Plain HTML form, typically `mailto:` or a controller POST action |
| `@/assets/foo.jpg` imports | `/img/foo.jpg` static path (copy file into `wwwroot/img/`) |
| `.avif` / `.jpg` / `.webp` | Copy as-is. Keep the same filename so `Var2Services`/`ExtraServices` lookups still work. |

### 5. Apply changes

Order matters — do things that unblock other work first:

1. **Copy new image assets** first (so views referencing them don't 404 during testing).
2. **Apply small bulk changes** (email domain, keywords, text edits) directly with Edit.
3. **Delegate big rewrites** — use the Agent tool with `subagent_type: general-purpose`, `model: sonnet`, one agent per page. Include a pointer to the existing similar Razor view as a pattern reference.
4. **Add routes/redirects** in `HomeController.cs`. New pages get full actions; redirects use `RedirectPermanent("/new-path")`.
5. **Update sitemap.xml** — add new URLs, remove deleted ones, match priorities from the React sitemap.
6. **Update tracking files**: set `.lovable-sync` `last_migrated_commit` to the HEAD hash you just migrated. Run `db-sync.mjs --write-state` for the database content (step 2b).

### 6. Verify

```bash
dotnet build EndReinigung/EndReinigung.csproj
```

Build the project-specific csproj, not the solution — the solution build often fails because the dev server (`EndreinigungZurich`) holds file locks on DLLs. File-lock errors on `AppServices.dll`/`DbAccess.dll` are **not** compile errors; ignore them and check the EndReinigung-specific output.

Expected: `0 Warning(s), 0 Error(s)`. Razor syntax errors (bad `@` escaping, unclosed tags) show up here.

For UI/functional verification, start the dev server and spot-check the changed pages in a browser — type checking only catches syntax, not visual regressions.

### 6b. Compare meta titles & descriptions with Lovable

[`meta-compare.mjs`](meta-compare.mjs) compares `<title>` and `<meta name="description">` for every URL in both sitemaps. It also uses git history on both sides to say which side changed each differing text last.

1. **Start our site:** `dotnet watch` (Ctrl+F6), which serves `http://localhost:5031`.
2. **Start Lovable's *current code*:**

   ```bash
   cd rich-lease-shine
   npm install --no-save --package-lock=false   # once; package-lock.json is stale (Lovable uses bun), don't rewrite it
   npx vite --port 8089 --strictPort --host 127.0.0.1    # run in the background
   ```

   Do **not** compare against `rich-lease-shine.lovable.app`. The published site lags behind the code: on 2026-11-06 it still showed older titles than Lovable's own source.
3. **Run it from the repo root:**

   ```bash
   node .claude/skills/lovable-migration/meta-compare.mjs     # ~5 min, renders Lovable with headless Chrome
   ```

4. **Read the verdict on each difference:**
   - **`LOVABLE NEWER`:** migrate it.
   - **`ours newer`:** leave it.
   - **`?`:** check by hand.

   **Deliberate divergence:** on 2026-05-17 (`00c097b` "updated meta titles") the user rewrote ~40 of our titles into a "✓ Fixpreis …" style. Ratgeber titles were suffixed "| Ratgeber Zürich Endreinigung" at the same time. Those are intentionally different from Lovable. Never "fix" them back unless Lovable changed that text *after* 2026-05-17.
5. **Stop the vite server** when done. Then run `git -C rich-lease-shine checkout -- supabase/functions/mcp/index.ts`: Lovable's `@lovable.dev/mcp-js` vite plugin rewrites that generated file on startup, and almost empties it. Finish with `git -C rich-lease-shine status` clean. Never commit in or push the Lovable repo.

Gotchas the script already handles (keep them if you change it):

- **Language:** Lovable stores the language in `localStorage` (`preferred-language`). After any `/en/...` page, a reused browser profile renders every later page in English, so the script uses a fresh profile per page.
- **Description source:** Lovable's `index.html` ships a default description. The page's real one is the `<meta … data-rh="true">` tag from react-helmet. If no `data-rh` tag is present, the page didn't finish rendering, or the route no longer exists in Lovable.
- **Browser:** use Chrome. `msedge.exe` started from node hands off to a broker process and returns empty output.
- **Encoding:** our HTML encodes umlauts as `&#xFC;`, so decode before comparing. Descriptions can contain `'` (e.g. `CHF 1'289`).
- **Unreliable dates:** dates are the last commit whose diff contains the exact text. `?`/`~` means only a fragment matched, or the text is built from data (Bezirk templates) or comes from the DB (Ratgeber, see step 2b).

### 7. Update the sync tracker

```bash
cd rich-lease-shine && NEW_HASH=$(git log -1 --format=%H)
```

Edit `.lovable-sync`:

```
last_migrated_date=YYYY-MM-DD
last_migrated_commit=<NEW_HASH>
```

Keep a commented-out history of prior baselines in the file — helps if a migration needs to be retried.

## Common pitfalls

- **Path to the Lovable clone**: always use `<repo>/rich-lease-shine` (the submodule). There is an unrelated `C:\Users\STN-Work\source\repos\rich-lease-shine` *next to* the repo, so `..\rich-lease-shine` from the repo root is the wrong folder.
- **Domain mismatch**: canonical URLs use `zuerich-endreinigung.ch` (with "ue"), but the email address and primary domain are `zurich-endreinigung.ch` (without "e"). Don't conflate them.
- **FAQ HTML in Razor**: When an FAQ answer contains HTML (e.g. an internal link), the view must render it via `@Html.Raw(faq.A)` — otherwise the `<a>` tag is HTML-encoded and shown as text.
- **`@` escaping in JSON-LD**: `@context` / `@type` must be written as `@@context` / `@@type` inside a `@section Head { ... }` block, because Razor treats `@` as a code marker.
- **Don't delete Razor views the user hasn't signed off on** — React removing `LP2.tsx` doesn't mean the `/lp2` URL is dead; users may still link to it, and removing it is destructive.
- **Private Lovable repo**: cloning requires a PAT. If `git clone` says "Repository not found" and the user says it's connected to GitHub, the repo is private — ask for a token or use `gh repo clone`.
- **Ratgeber articles and Google reviews** are loaded from Supabase in React. Here they are JSON snapshots in `App_Data/`. Keep them in sync with step 2b. The `supabase/migrations/*.sql` files are **not** the complete history, so always compare against the live DB.
- **Supabase-backed React components** (`useEffect` + `supabase.from(...)`) start out with a hard-coded `fallback…` array. Never port that fallback as the content. It is what React shows only when the DB call fails. (The review sections here shipped with only the single fallback review "Emil" until 2026-11-06.)
