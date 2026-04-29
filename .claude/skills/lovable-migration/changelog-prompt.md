# Lovable Changelog Prompt

Paste the prompt below into Lovable to get a structured summary of all changes since the last migrated commit.

**Before pasting**: replace `<BASELINE_COMMIT>` with the value of `last_migrated_commit` from [`.lovable-sync`](../../../.lovable-sync), and `<BASELINE_DATE>` with `last_migrated_date`.

---

I need a detailed changelog of everything that changed in this repo since commit `2ff6adbb170358f7c6b9c37b7eccdcb0ee97a152` (2026-04-20). The output is being used to manually port changes into a separate ASP.NET Razor mirror of the site, so I need **intent and behavior**, not a `git diff` dump. Be exhaustive — a missed item causes a visible regression on the production site.

Organize the response into these sections. Omit any section that has no changes.

**1. New pages**
For each new page under `src/pages/`: route path, page title, one-paragraph purpose, list of major sections in render order, any data sources (static, props, Supabase, API), and any SEO/meta tags set via Helmet.

**2. Deleted or renamed pages**
Old path → new path (or "removed"). If removed, say whether the URL should 404, redirect, or stay reachable.

**3. Route / redirect changes in `src/App.tsx`**
New `<Route>` entries, removed entries, and any redirect rules. Include path, component, and whether it's a permanent redirect.

**4. Existing pages — structural / content changes**
Per page that changed: a bullet list of what changed in plain language (e.g., "added testimonial slider above FAQ", "replaced 3-column pricing with 2-column", "rewrote hero copy"). Distinguish copy edits from layout/component changes.

**5. New / changed shared components**
Component name, where it's used, props, and behavior. Flag anything stateful (`useState`, `useEffect`, animations, scroll-triggered behavior, modals, accordions, sliders) — describe the interaction in enough detail to reimplement without React.

**6. Copy, SEO, and metadata**
All text copy edits (German + any other language), changes to `<title>`, meta description, meta keywords, canonical URL, Open Graph tags, and JSON-LD schema (FAQ, LocalBusiness, etc.) — quote the new values verbatim.

**7. Footer and navigation**
Any link added, removed, or relabeled. Any URL rewrites.

**8. Styling and layout**
Tailwind class changes that affect layout/spacing/breakpoints/typography (not pure cosmetic tweaks). Color or theme variable changes. New responsive behavior.

**9. Assets**
List every file added to or removed from `src/assets/` and `public/`, with the filename and where it's referenced. Also list any image swaps (same filename, new content).

**10. Forms, validation, and submissions**
Any form added or changed: fields, validation rules, submit target (mailto, Supabase, API endpoint), success/error UX.

**11. Dynamic / external data**
Anything that pulls from Supabase, an API, or browser APIs (geolocation, localStorage, etc.). Be explicit — these don't exist in the Razor mirror and need a workaround.

**12. Sitemap / robots.txt**
Any change to `public/sitemap.xml` or `public/robots.txt`: URLs added, removed, or with changed priority/changefreq.

**13. Dependencies**
New packages in `package.json` and what they're used for.

**14. Anything else**
Config changes, build tweaks, env vars, i18n keys added/removed.

Be specific with file paths. If you're unsure whether something changed, list it anyway and mark it "verify".

---

## Follow-up prompt for interactivity gaps

If a migrated page feels off after applying the changelog, ask Lovable:

> For the [page] page, walk me through every user interaction from first paint to scroll-to-bottom — what triggers what, in what order. Include `useEffect` dependencies, animation triggers, and any state that changes based on scroll position or viewport.
