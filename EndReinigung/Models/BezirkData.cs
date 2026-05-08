namespace EndReinigung.Models
{
    public sealed record Gemeinde(string Name, string Plz);

    public sealed record Bezirk(string Slug, string Name, string Hauptort, IReadOnlyList<Gemeinde> Gemeinden);

    public static class BezirkData
    {
        public static readonly IReadOnlyList<Bezirk> All = new[]
        {
            new Bezirk("affoltern", "Bezirk Affoltern", "Affoltern am Albis", new[]
            {
                new Gemeinde("Aeugst am Albis", "8914"),
                new Gemeinde("Affoltern am Albis", "8910"),
                new Gemeinde("Bonstetten", "8906"),
                new Gemeinde("Hausen am Albis", "8915"),
                new Gemeinde("Hedingen", "8908"),
                new Gemeinde("Kappel am Albis", "8926"),
                new Gemeinde("Knonau", "8934"),
                new Gemeinde("Maschwanden", "8933"),
                new Gemeinde("Mettmenstetten", "8932"),
                new Gemeinde("Obfelden", "8912"),
                new Gemeinde("Ottenbach", "8913"),
                new Gemeinde("Rifferswil", "8911"),
                new Gemeinde("Stallikon", "8143"),
                new Gemeinde("Wettswil am Albis", "8907"),
            }),
            new Bezirk("andelfingen", "Bezirk Andelfingen", "Andelfingen", new[]
            {
                new Gemeinde("Adlikon", "8452"),
                new Gemeinde("Andelfingen", "8450"),
                new Gemeinde("Benken", "8463"),
                new Gemeinde("Berg am Irchel", "8415"),
                new Gemeinde("Dachsen", "8447"),
                new Gemeinde("Feuerthalen", "8245"),
                new Gemeinde("Flaach", "8416"),
                new Gemeinde("Flurlingen", "8247"),
                new Gemeinde("Henggart", "8444"),
                new Gemeinde("Humlikon", "8457"),
                new Gemeinde("Kleinandelfingen", "8451"),
                new Gemeinde("Laufen-Uhwiesen", "8248"),
                new Gemeinde("Marthalen", "8460"),
                new Gemeinde("Ossingen", "8475"),
                new Gemeinde("Rheinau", "8462"),
                new Gemeinde("Thalheim an der Thur", "8478"),
                new Gemeinde("Trüllikon", "8466"),
                new Gemeinde("Truttikon", "8467"),
                new Gemeinde("Volken", "8459"),
            }),
            new Bezirk("buelach", "Bezirk Bülach", "Bülach", new[]
            {
                new Gemeinde("Bülach", "8180"),
                new Gemeinde("Bachenbülach", "8184"),
                new Gemeinde("Bassersdorf", "8303"),
                new Gemeinde("Dietlikon", "8305"),
                new Gemeinde("Embrach", "8424"),
                new Gemeinde("Glattfelden", "8192"),
                new Gemeinde("Höri", "8181"),
                new Gemeinde("Kloten", "8302"),
                new Gemeinde("Nürensdorf", "8309"),
                new Gemeinde("Oberembrach", "8425"),
                new Gemeinde("Opfikon", "8152"),
                new Gemeinde("Rafz", "8197"),
                new Gemeinde("Rorbas", "8427"),
                new Gemeinde("Wallisellen", "8304"),
                new Gemeinde("Wasterkingen", "8195"),
                new Gemeinde("Wil", "8196"),
                new Gemeinde("Winkel", "8185"),
            }),
            new Bezirk("dielsdorf", "Bezirk Dielsdorf", "Dielsdorf", new[]
            {
                new Gemeinde("Bachs", "8164"),
                new Gemeinde("Boppelsen", "8113"),
                new Gemeinde("Buchs", "8107"),
                new Gemeinde("Dällikon", "8108"),
                new Gemeinde("Dänikon", "8114"),
                new Gemeinde("Dielsdorf", "8157"),
                new Gemeinde("Hüttikon", "8115"),
                new Gemeinde("Neerach", "8173"),
                new Gemeinde("Niederglatt", "8172"),
                new Gemeinde("Niederhasli", "8155"),
                new Gemeinde("Oberglatt", "8154"),
                new Gemeinde("Otelfingen", "8112"),
                new Gemeinde("Regensberg", "8158"),
                new Gemeinde("Regensdorf", "8105"),
                new Gemeinde("Rümlang", "8153"),
                new Gemeinde("Stadel", "8174"),
                new Gemeinde("Steinmaur", "8162"),
                new Gemeinde("Weiach", "8187"),
            }),
            new Bezirk("dietikon", "Bezirk Dietikon", "Dietikon", new[]
            {
                new Gemeinde("Aesch", "8904"),
                new Gemeinde("Birmensdorf", "8903"),
                new Gemeinde("Dietikon", "8953"),
                new Gemeinde("Geroldswil", "8954"),
                new Gemeinde("Oberengstringen", "8102"),
                new Gemeinde("Oetwil an der Limmat", "8955"),
                new Gemeinde("Schlieren", "8952"),
                new Gemeinde("Spreitenbach", "8957"),
                new Gemeinde("Uitikon", "8142"),
                new Gemeinde("Unterengstringen", "8103"),
                new Gemeinde("Urdorf", "8902"),
                new Gemeinde("Weiningen", "8104"),
            }),
            new Bezirk("hinwil", "Bezirk Hinwil", "Hinwil", new[]
            {
                new Gemeinde("Bäretswil", "8344"),
                new Gemeinde("Bubikon", "8608"),
                new Gemeinde("Dürnten", "8635"),
                new Gemeinde("Gossau", "8625"),
                new Gemeinde("Grüningen", "8627"),
                new Gemeinde("Hinwil", "8340"),
                new Gemeinde("Rüti", "8630"),
                new Gemeinde("Seegräben", "8607"),
                new Gemeinde("Wetzikon", "8620"),
            }),
            new Bezirk("horgen", "Bezirk Horgen", "Horgen", new[]
            {
                new Gemeinde("Adliswil", "8134"),
                new Gemeinde("Hirzel", "8816"),
                new Gemeinde("Horgen", "8810"),
                new Gemeinde("Hütten", "8825"),
                new Gemeinde("Kilchberg", "8802"),
                new Gemeinde("Langnau am Albis", "8135"),
                new Gemeinde("Oberrieden", "8824"),
                new Gemeinde("Richterswil", "8805"),
                new Gemeinde("Rüschlikon", "8803"),
                new Gemeinde("Thalwil", "8800"),
                new Gemeinde("Wädenswil", "8820"),
            }),
            new Bezirk("meilen", "Bezirk Meilen", "Meilen", new[]
            {
                new Gemeinde("Erlenbach", "8703"),
                new Gemeinde("Herrliberg", "8704"),
                new Gemeinde("Hombrechtikon", "8634"),
                new Gemeinde("Küsnacht", "8700"),
                new Gemeinde("Männedorf", "8708"),
                new Gemeinde("Meilen", "8706"),
                new Gemeinde("Oetwil am See", "8618"),
                new Gemeinde("Stäfa", "8712"),
                new Gemeinde("Uetikon am See", "8707"),
                new Gemeinde("Zollikon", "8702"),
                new Gemeinde("Zumikon", "8126"),
            }),
            new Bezirk("pfaeffikon", "Bezirk Pfäffikon", "Pfäffikon ZH", new[]
            {
                new Gemeinde("Bauma", "8494"),
                new Gemeinde("Fehraltorf", "8320"),
                new Gemeinde("Hittnau", "8335"),
                new Gemeinde("Illnau-Effretikon", "8307"),
                new Gemeinde("Kyburg", "8314"),
                new Gemeinde("Lindau", "8315"),
                new Gemeinde("Pfäffikon", "8330"),
                new Gemeinde("Russikon", "8332"),
                new Gemeinde("Weisslingen", "8484"),
                new Gemeinde("Wila", "8492"),
                new Gemeinde("Wildberg", "8489"),
            }),
            new Bezirk("uster", "Bezirk Uster", "Uster", new[]
            {
                new Gemeinde("Uster", "8610"),
                new Gemeinde("Dübendorf", "8600"),
                new Gemeinde("Egg", "8132"),
                new Gemeinde("Fällanden", "8117"),
                new Gemeinde("Greifensee", "8606"),
                new Gemeinde("Maur", "8124"),
                new Gemeinde("Mönchaltorf", "8617"),
                new Gemeinde("Schwerzenbach", "8603"),
                new Gemeinde("Volketswil", "8604"),
                new Gemeinde("Wangen-Brüttisellen", "8602"),
            }),
            new Bezirk("winterthur", "Bezirk Winterthur", "Winterthur", new[]
            {
                new Gemeinde("Winterthur", "8400"),
                new Gemeinde("Elsau", "8352"),
                new Gemeinde("Illnau-Effretikon", "8307"),
                new Gemeinde("Lindau", "8315"),
                new Gemeinde("Neftenbach", "8413"),
                new Gemeinde("Rickenbach", "8545"),
                new Gemeinde("Seuzach", "8472"),
                new Gemeinde("Turbenthal", "8488"),
                new Gemeinde("Wiesendangen", "8542"),
                new Gemeinde("Zell", "8468"),
            }),
            new Bezirk("zuerich", "Bezirk Zürich", "Zürich", new[]
            {
                new Gemeinde("Zürich", "8000"),
            }),
        };

        public static Bezirk? FindBySlug(string slug) =>
            All.FirstOrDefault(b => b.Slug == slug);
    }
}
