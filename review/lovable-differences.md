# Open differences: our site vs. Lovable (as of 6 Nov 2026)

Everything below is **not changed yet** and needs a decision. Items are roughly in order of importance.
"Lovable" means Lovable's current code (`rich-lease-shine` @ `33b568c`) and its live database, not the published `rich-lease-shine.lovable.app` site (that one is out of date).

Meta titles and descriptions for every page are in [meta-titles-descriptions.csv](meta-titles-descriptions.csv) (opens in Excel; last column is for your notes).

---

## 1. Meta titles & descriptions

Full list in the CSV. Summary: 36 of 82 pages identical. Almost all other differences are **your own 17 May SEO rewrite** (`00c097b` "updated meta titles", the "✓ Fixpreis …" titles and the "| Ratgeber Zürich Endreinigung" suffix), which is newer than Lovable's text. Those were left alone.

To decide:

- [ ] **Dienstleistungen description: Lovable is newer** (Lovable 26 Apr, ours 7 Apr).
  - Ours: *Professionelle Reinigungsservices in Zürich: Baureinigung, Fensterreinigung, Teppichreinigung, Polsterreinigung, Airbnb und mehr. Jetzt Offerte anfordern.*
  - Lovable: *Über 14 Reinigungsservices in Zürich zum Fixpreis: Endreinigung, Frühlingsputz, Fenster, Polster, Teppich, Hochdruck, Airbnb, Büro u.v.m. Inkl. Anfahrt, 5 Mio. CHF versichert.*
- [ ] **"Bezirk Zürich" vs "Stadt Zürich"** on `/baureinigung/zuerich` and `/fruehlingsputz-bezirk-zuerich` (title, description and page text).
  - Lovable has used "Stadt Zürich" since March; ours says "Bezirk Zürich" since our port in May.
  - Lovable's description is ungrammatical ("im Stadt Zürich"). If we switch, write "in der Stadt Zürich".
- [ ] **Gewerbereinigung description**: Lovable says "Baureinigung ab **CHF 345**", ours "ab **CHF 435**". 435 matches the Baureinigung prices everywhere else, so Lovable's is probably a typo. Keep ours?
- [ ] **Ratgeber article descriptions**: our template shortens the intro with "…" (e.g. Abmeldung); Lovable shows the full intro.
- [ ] **Title length**: 47 of our titles are longer than ~60 characters, where Google usually cuts them off (column "Our title length" in the CSV).

## 2. Ratgeber articles (Lovable database)

Lovable edits article content directly in its database; that never shows up in git. Today two stale articles were synced (Endreinigung Kosten: 3 → 41 sections; Abmeldung bei der Gemeinde: rewritten in "Sie" form + English). Still open:

- [ ] **Wohnungsübergabe Checkliste Zürich 2026**: German content differs from Lovable's live version:
  - Link: ours `/umzugsreinigung-stadt-zuerich`, Lovable `/umzugsreinigung-zuerich` (ours is probably a deliberate fix to our own URL; keep).
  - One tip text differs. Ours: *"Fotografieren Sie jeden Raum vor und nach der Reinigung…"*. Lovable: *"Bringen Sie zur Übergabe eigene Putzutensilien (Mikrofasertuch, Entkalker, Glasreiniger)…"*.
  - Price in the "Was kostet…" section: ours **CHF 800**, Lovable **CHF 850**.
  - We have one extra tip block before the FAQ (*"Machen Sie vor und nach der Reinigung Fotos von jedem Raum…"*) that Lovable no longer has.
  - Suggestion: take Lovable's version but keep our link.
- [ ] **English text missing for 8 articles** that Lovable has in English: endreinigung-kosten-zuerich, endreinigung-selber-machen-oder-firma, hauswart-abnahme-pruefung, endreinigung-tipps-expats-zuerich, baureinigung-zuerich-kosten, polsterreinigung-zuerich-preise-2026, fensterreinigung-zuerich-tipps-preise, umzug-zuerich-checkliste-2026. (Lovable's English for "endreinigung-kosten-zuerich" is still the old short draft.)
- [ ] **Database sync baseline not recorded yet.** After the two points above are decided, run `node .claude/skills/lovable-migration/db-sync.mjs --write-state` and commit `.lovable-db-sync.json`. Future migrations then report exactly what changed in Lovable's database.

## 3. Page design

- [ ] **"Kurz erklärt" box**: Lovable uses one shared design (thin top line, ✨ label, dotted bullets). Done today for Standorte and Umzugsreinigung.
  - About 15 other service pages still have the older boxed design (Baureinigung, Grundreinigung, Fensterreinigung, …).
  - Some pages have no box at all (Gewerbereinigung, Spezialreinigung, Bezirk pages).
  - Suggestion: one shared partial view so the next design change is made once.
- [ ] **Google line under the Umzugsreinigung hero**: "5/5 auf Google" vs "5/5 Bewertung auf Google" on the home page. Make them the same?

## 4. Housekeeping

- [ ] **`.claude/settings.local.json` is tracked in git** although it is a personal file. It was **not** committed today: it now allows every shell command without asking, and it contains Lovable's Supabase key. Suggestion: stop tracking it (`git rm --cached`) and add it to `.gitignore`.
- [ ] **Two old copies of the site are still running** on `localhost:5193` and `localhost:5194` (old builds). The current one is `dotnet watch` on 5031. Stop them?
- Info: **Google reviews** are now a snapshot (`EndReinigung/App_Data/google-reviews.json`, 12 reviews). New Google reviews only appear after a re-sync: `node .claude/skills/lovable-migration/db-sync.mjs --apply-reviews`.
- Info: changes to `App_Data/*.json` only show after a restart. The site reads them once at startup from the build folder, and `dotnet watch` doesn't pick them up.
