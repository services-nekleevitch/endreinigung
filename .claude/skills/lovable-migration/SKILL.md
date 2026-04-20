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
6. **Update tracking file** — set `.lovable-sync` `last_migrated_commit` to the HEAD hash you just migrated.

### 6. Verify

```bash
dotnet build EndReinigung/EndReinigung.csproj
```

Build the project-specific csproj, not the solution — the solution build often fails because the dev server (`EndreinigungZurich`) holds file locks on DLLs. File-lock errors on `AppServices.dll`/`DbAccess.dll` are **not** compile errors; ignore them and check the EndReinigung-specific output.

Expected: `0 Warning(s), 0 Error(s)`. Razor syntax errors (bad `@` escaping, unclosed tags) show up here.

For UI/functional verification, start the dev server and spot-check the changed pages in a browser — type checking only catches syntax, not visual regressions.

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

- **Domain mismatch**: canonical URLs use `zuerich-endreinigung.ch` (with "ue"), but the email address and primary domain are `zurich-endreinigung.ch` (without "e"). Don't conflate them.
- **FAQ HTML in Razor**: When an FAQ answer contains HTML (e.g. an internal link), the view must render it via `@Html.Raw(faq.A)` — otherwise the `<a>` tag is HTML-encoded and shown as text.
- **`@` escaping in JSON-LD**: `@context` / `@type` must be written as `@@context` / `@@type` inside a `@section Head { ... }` block, because Razor treats `@` as a code marker.
- **Don't delete Razor views the user hasn't signed off on** — React removing `LP2.tsx` doesn't mean the `/lp2` URL is dead; users may still link to it, and removing it is destructive.
- **Private Lovable repo**: cloning requires a PAT. If `git clone` says "Repository not found" and the user says it's connected to GitHub, the repo is private — ask for a token or use `gh repo clone`.
- **Ratgeber articles** (`/ratgeber/<slug>`) in React are loaded dynamically from Supabase. The ASP.NET site has no Supabase, so these routes 404. Don't pretend to fix this — flag it to the user.
