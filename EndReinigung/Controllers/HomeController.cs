using EndReinigung.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EndReinigung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("extra-services")]
        public IActionResult ExtraServices()
        {
            return View();
        }

        [Route("fensterreinigung")]
        public IActionResult Fensterreinigung()
        {
            return View();
        }

        [Route("polsterreinigung")]
        public IActionResult Polsterreinigung()
        {
            return View();
        }

        [Route("tapeten-entfernen")]
        public IActionResult TapetenEntfernen()
        {
            return View();
        }

        [Route("lp2")]
        public IActionResult LP2()
        {
            return View();
        }

        [Route("calculator")]
        public IActionResult Calculator()
        {
            return View();
        }

        [Route("ratgeber")]
        public IActionResult Ratgeber()
        {
            return View();
        }

        [Route("ratgeber/{slug}")]
        public IActionResult RatgeberArticle(string slug)
        {
            var article = RatgeberArticleData.FindBySlug(slug);
            if (article == null)
            {
                return RedirectPermanent("/ratgeber");
            }
            return View(article);
        }

        [Route("abnahmegarantie")]
        public IActionResult Abnahmegarantie()
        {
            return View();
        }

        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [Route("garantie")]
        public IActionResult Garantie()
        {
            return View();
        }

        [Route("impressum")]
        public IActionResult Impressum()
        {
            return View();
        }

        [Route("agb")]
        public IActionResult AGB()
        {
            return View();
        }

        [Route("datenschutz")]
        public IActionResult Datenschutz()
        {
            return View();
        }

        [Route("ueber-uns")]
        public IActionResult UeberUns()
        {
            return View();
        }

        [Route("reinigung-horgen")]
        public IActionResult BezirkHorgen()
        {
            return View();
        }

        [Route("airbnb-reinigung-zuerich")]
        public IActionResult AirbnbReinigung()
        {
            return View();
        }

        [Route("en/airbnb-cleaning-zurich")]
        public IActionResult AirbnbCleaningEN()
        {
            return View("AirbnbReinigung");
        }

        [Route("en/upholstery-cleaning-zurich")]
        public IActionResult UpholsteryCleaning()
        {
            return View();
        }

        [Route("bueroreinigung")]
        public IActionResult Bueroreinigung()
        {
            return View();
        }

        [Route("praxisreinigung-zuerich")]
        public IActionResult Praxisreinigung()
        {
            return View();
        }

        [Route("praxisreinigung")]
        public IActionResult PraxisreinigungRedirect()
        {
            return RedirectPermanent("/praxisreinigung-zuerich");
        }

        [Route("baureinigung")]
        public IActionResult Baureinigung()
        {
            return View();
        }

        [Route("baureinigung/{slug}")]
        public IActionResult BaureinigungBezirk(string slug)
        {
            var bezirk = BezirkData.FindBySlug(slug);
            if (bezirk == null)
            {
                return RedirectPermanent("/baureinigung");
            }
            return View(bezirk);
        }

        [Route("baureinigung-calculator")]
        public IActionResult BaureinigunCalculator()
        {
            return RedirectToAction("Calculator");
        }

        [Route("teppichreinigung")]
        public IActionResult Teppichreinigung()
        {
            return View();
        }

        [Route("matratzenreinigung")]
        public IActionResult Matratzenreinigung()
        {
            return View();
        }

        [Route("grundreinigung")]
        public IActionResult Grundreinigung()
        {
            return View();
        }

        [Route("umzugsreinigung-zuerich")]
        public IActionResult Umzugsreinigung()
        {
            return View();
        }

        [Route("fruehlingsputz-zuerich")]
        public IActionResult Fruehlingsreinigung()
        {
            return View();
        }

        [Route("fruehlingsputz-bezirk-{slug}")]
        public IActionResult FruehlingsputzBezirk(string slug)
        {
            var bezirk = BezirkData.FindBySlug(slug);
            if (bezirk == null)
            {
                return RedirectPermanent("/fruehlingsputz-zuerich");
            }
            return View(bezirk);
        }

        [Route("umzugsreinigung-stadt-zuerich")]
        public IActionResult UmzugsreinigungStadtZuerich()
        {
            return View();
        }

        [Route("umzugsreinigung-bezirk-affoltern")]
        public IActionResult UmzugsreinigungBezirkAffoltern()
        {
            ViewData["BezirkName"] = "Bezirk Affoltern";
            ViewData["BezirkBadgeLabel"] = "Bezirk Affoltern";
            ViewData["H1"] = "Umzugsreinigung Bezirk Affoltern — Endreinigung mit Abnahmegarantie im Knonaueramt";
            ViewData["H2"] = "Reinigungsfirma im Säuliamt — lokal verwurzelt, mit Abnahmegarantie";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Affoltern";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Affoltern – Büro vor Ort in 8910";
            ViewData["ExamplePlz"] = "8910";
            ViewData["Title"] = "Umzugsreinigung Bezirk Affoltern | Knonaueramt | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Affoltern (Knonaueramt) — alle 14 Gemeinden, Endreinigung mit Abnahmegarantie, persönliche Wohnungsabgabe. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-affoltern";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-affoltern-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Affoltern";
            ViewData["BoundaryPhrase"] = "von Aeugst am Albis bis Maschwanden";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Affoltern</strong> ist unser Heimspiel. Wir decken das gesamte Knonaueramt ab — <strong>von Aeugst am Albis bis Maschwanden</strong>, von Bonstetten bis Rifferswil. Unser Büro steht in 8910 Affoltern am Albis.";
            ViewData["LogisticsParagraph"] = "Wir kennen die Hausverwaltungen im Säuliamt persönlich — <strong>Genossenschaften wie die ABZ in Bonstetten, regionale Verwaltungen in Affoltern und Mettmenstetten</strong> sowie die kleineren privaten Liegenschaftsverwaltungen in den Dörfern. Logistisch hat das Knonaueramt eigene Themen: enge Innenhof-Zufahrten in den Dorfkernen von Hedingen und Mettmenstetten, schmale Erschliessungsstrassen in Kappel, und in Obfelden kommen wir mit unseren Reinigungsfahrzeugen oft direkt an die Hauseingänge — was Zeit spart und die Wohnungsabgabe entspannter macht.";
            ViewData["RegionalAvailability"] = "Im gesamten Knonaueramt bieten wir neben der Umzugsreinigung auch <strong>Abo-Reinigungen für Mehrfamilienhäuser, Fenster- und Storenreinigung sowie Spezialreinigungen</strong> nach Renovation oder Wasserschaden an. Als lokale Reinigungsfirma sind wir oft innerhalb von 30 Minuten beim Kunden.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen 14 Gemeinden im Knonaueramt";
            ViewData["Gemeinden"] = "Aeugst am Albis:8914,Affoltern am Albis:8910,Bonstetten:8906,Hausen am Albis:8915,Hedingen:8908,Kappel am Albis:8926,Knonau:8934,Maschwanden:8933,Mettmenstetten:8932,Obfelden:8912,Ottenbach:8913,Rifferswil:8911,Stallikon:8143,Wettswil am Albis:8907";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-andelfingen")]
        public IActionResult UmzugsreinigungBezirkAndelfingen()
        {
            ViewData["BezirkName"] = "Bezirk Andelfingen";
            ViewData["BezirkBadgeLabel"] = "Bezirk Andelfingen";
            ViewData["H1"] = "Umzugsreinigung Bezirk Andelfingen — Endreinigung mit Abnahmegarantie im Weinland";
            ViewData["H2"] = "Reinigungsfirma im Zürcher Weinland — vom Riegelbau bis zum Bauernhaus";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Andelfingen";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Andelfingen – im ganzen Zürcher Weinland";
            ViewData["ExamplePlz"] = "8450";
            ViewData["Title"] = "Umzugsreinigung Bezirk Andelfingen | Zürcher Weinland | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Andelfingen — Endreinigung mit Abnahmegarantie im Zürcher Weinland. Vom Riegelbau bis zum modernen Wohnhaus. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-andelfingen";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-andelfingen-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Andelfingen";
            ViewData["BoundaryPhrase"] = "von Adlikon bis Volken, von Rheinau bis Stammheim";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Andelfingen</strong> deckt das gesamte Zürcher Weinland ab — <strong>von Adlikon bis Volken, von Rheinau bis Stammheim</strong>. Wir kennen jede Gemeinde zwischen Thur und Rhein.";
            ViewData["LogisticsParagraph"] = "Im Weinland haben wir es oft mit kleineren, persönlichen <strong>Hausverwaltungen und privaten Vermietern</strong> zu tun, die ihre Liegenschaften noch selbst übergeben. Das heisst: Die Wohnungsabgabe wird gründlich, manchmal akribisch geprüft — gerade bei alten Riegelhäusern in Flaach oder Bauernhäusern in Kleinandelfingen. Logistisch sind die Anfahrten länger, aber dafür sind die Parksituationen entspannt: In den meisten Dörfern können wir direkt vor dem Haus halten und brauchen keine Bewilligungen für Parkplätze oder Lieferzonen.";
            ViewData["RegionalAvailability"] = "In der gesamten Region Andelfingen bieten wir neben der Umzugsreinigung auch <strong>Fensterreinigung, Reinigung von Wintergärten und Terrassen sowie Grundreinigungen nach Renovationen</strong> an. Auch Reinigungs-Abos für Ferienwohnungen und Bed & Breakfast-Betriebe im Weinland gehören dazu.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Zürcher Weinlands";
            ViewData["Gemeinden"] = "Adlikon:8106,Andelfingen:8450,Benken:8463,Berg am Irchel:8415,Buch am Irchel:8414,Dachsen:8447,Dorf:8458,Feuerthalen:8245,Flaach:8416,Flurlingen:8247,Henggart:8444,Humlikon:8457,Kleinandelfingen:8451,Laufen-Uhwiesen:8248,Marthalen:8460,Oberstammheim:8477,Ossingen:8475,Rheinau:8462,Thalheim an der Thur:8478,Trüllikon:8466,Truttikon:8467,Unterstammheim:8476,Volken:8459";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-buelach")]
        public IActionResult UmzugsreinigungBezirkBuelach()
        {
            ViewData["BezirkName"] = "Bezirk Bülach";
            ViewData["BezirkBadgeLabel"] = "Bezirk Bülach";
            ViewData["H1"] = "Umzugsreinigung Bezirk Bülach — Endreinigung mit Abnahmegarantie im Unterland";
            ViewData["H2"] = "Reinigungsfirma im Zürcher Unterland — von Kloten bis Embrach";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Bülach";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Bülach – Flughafenregion und Unterland";
            ViewData["ExamplePlz"] = "8180";
            ViewData["Title"] = "Umzugsreinigung Bezirk Bülach | Zürcher Unterland | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Bülach — Endreinigung mit Abnahmegarantie im Zürcher Unterland. Kloten, Opfikon, Bülach, Embrach. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-buelach";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-buelach-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Bülach";
            ViewData["BoundaryPhrase"] = "von Bachenbülach bis Wil, von Rafz bis Wallisellen";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Bülach</strong> deckt das gesamte Zürcher Unterland ab — <strong>von Bachenbülach bis Wil, von Rafz bis Wallisellen</strong>. Wir reinigen in der Flughafenregion und in jeder Unterland-Gemeinde.";
            ViewData["LogisticsParagraph"] = "Im Bezirk Bülach kennen wir die grossen Verwaltungen rund um die <strong>Flughafenregion</strong> — Kloten, Opfikon, Glattpark — und die expat-orientierten Vermieter, die Wohnungen oft kurzfristig übergeben. Logistisch sind die Hochhaus-Überbauungen in Glattpark und The Circle anspruchsvoll: Wir brauchen Anmeldungen für Tiefgaragen, Liftreservationen und müssen Reinigungsmaterial in einem Gang ins 12. Stockwerk transportieren. In den ländlicheren Gemeinden wie Hüntwangen, Rafz und Eglisau sind die Zufahrten dagegen unkompliziert.";
            ViewData["RegionalAvailability"] = "In der ganzen Flughafenregion bieten wir neben der Umzugsreinigung auch <strong>Büro- und Praxisreinigungen, Hochdruckreinigung, Fenster- und Storenreinigung sowie Abo-Reinigungen</strong> für Privat- und Geschäftskunden an. Besonders gefragt bei den vielen internationalen Mietern in Kloten und Opfikon.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Zürcher Unterlands";
            ViewData["Gemeinden"] = "Bachenbülach:8184,Bassersdorf:8303,Bülach:8180,Dietlikon:8305,Eglisau:8193,Embrach:8424,Freienstein-Teufen:8427,Glattfelden:8192,Hochfelden:8182,Höri:8181,Hüntwangen:8194,Kloten:8302,Lufingen:8426,Nürensdorf:8309,Oberembrach:8425,Opfikon:8152,Rafz:8197,Rorbas:8427,Wallisellen:8304,Wasterkingen:8195,Wil:8196,Winkel:8185";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-dielsdorf")]
        public IActionResult UmzugsreinigungBezirkDielsdorf()
        {
            ViewData["BezirkName"] = "Bezirk Dielsdorf";
            ViewData["BezirkBadgeLabel"] = "Bezirk Dielsdorf";
            ViewData["H1"] = "Umzugsreinigung Bezirk Dielsdorf — Endreinigung mit Abnahmegarantie im Furttal & Wehntal";
            ViewData["H2"] = "Reinigungsfirma zwischen Stadt und Land — Bezirk Dielsdorf";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Dielsdorf";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Dielsdorf – Furttal, Wehntal und Rafzerfeld-Vorland";
            ViewData["ExamplePlz"] = "8157";
            ViewData["Title"] = "Umzugsreinigung Bezirk Dielsdorf | Furttal & Wehntal | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Dielsdorf — Endreinigung mit Abnahmegarantie im Furttal und Wehntal. Regensdorf, Niederweningen, Dielsdorf. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-dielsdorf";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-dielsdorf-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Dielsdorf";
            ViewData["BoundaryPhrase"] = "von Otelfingen im Furttal bis Weiach am Rhein";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Dielsdorf</strong> spannt sich <strong>von Otelfingen im Furttal bis Weiach am Rhein</strong>. Wir kennen jede Gemeinde zwischen Lägern und Wehntal.";
            ViewData["LogisticsParagraph"] = "Im Bezirk Dielsdorf treffen wir auf eine spannende Mischung: <strong>Grössere institutionelle Verwaltungen in Regensdorf und Rümlang</strong>, dazu viele kleinere lokale Hausverwaltungen in den Wehntaler Dörfern, die ihre Liegenschaften persönlich übergeben. Logistisch ist Regensdorf-Watt mit seinen grossen Wohnsiedlungen gut erschlossen — Anwohnerparkkarten, Liftreservationen und Müll-Container-Zugang müssen aber im Voraus geklärt werden. In Boppelsen, Bachs und Steinmaur können wir dagegen direkt am Hauseingang halten.";
            ViewData["RegionalAvailability"] = "In der gesamten Region Dielsdorf bieten wir neben der Umzugsreinigung auch <strong>Fensterreinigung, Treppenhaus-Abos für Mehrfamilienhäuser und Bauschlussreinigung</strong> für die vielen Neubauprojekte im Furttal an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden im Furttal und Wehntal";
            ViewData["Gemeinden"] = "Bachs:8164,Boppelsen:8113,Buchs:8107,Dällikon:8108,Dänikon:8114,Dielsdorf:8157,Hüttikon:8115,Neerach:8173,Niederglatt:8172,Niederhasli:8155,Niederweningen:8166,Oberglatt:8154,Oberweningen:8165,Otelfingen:8112,Regensberg:8158,Regensdorf:8105,Rümlang:8153,Schleinikon:8165,Schöfflisdorf:8165,Stadel:8174,Steinmaur:8162,Weiach:8187";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-dietikon")]
        public IActionResult UmzugsreinigungBezirkDietikon()
        {
            ViewData["BezirkName"] = "Bezirk Dietikon";
            ViewData["BezirkBadgeLabel"] = "Bezirk Dietikon";
            ViewData["H1"] = "Umzugsreinigung Bezirk Dietikon — Endreinigung mit Abnahmegarantie im Limmattal";
            ViewData["H2"] = "Reinigungsfirma im Limmattal — urban, effizient, mit Garantie";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Dietikon";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Dietikon – im ganzen Limmattal";
            ViewData["ExamplePlz"] = "8953";
            ViewData["Title"] = "Umzugsreinigung Bezirk Dietikon | Limmattal | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Dietikon — Endreinigung mit Abnahmegarantie im Limmattal. Dietikon, Schlieren, Urdorf, Birmensdorf. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-dietikon";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-dietikon-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Dietikon";
            ViewData["BoundaryPhrase"] = "von Aesch und Birmensdorf bis Weiningen an der Limmat";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Dietikon</strong> deckt das gesamte Limmattal ab — <strong>von Aesch und Birmensdorf bis Weiningen an der Limmat</strong>. Wir sind in jeder Gemeinde regelmässig im Einsatz.";
            ViewData["LogisticsParagraph"] = "Das Limmattal ist eine der dichtest besiedelten Regionen des Kantons — und das merken wir bei der Logistik. <strong>Anwohnerparkkarten in Dietikon und Schlieren</strong>, knappe Lieferzonen vor Hochhaus-Überbauungen und Liftreservationen für die grossen Wohnsiedlungen sind Standard. Wir kennen die Verwaltungen wie <strong>Allreal, Mobimo, Pensimo</strong> und die regionalen Genossenschaften und wissen, welche Übergabe-Standards sie ansetzen. In den Hanggemeinden wie Uitikon und Birmensdorf sind die Zufahrten oft enger — wir kommen mit kleineren Servicefahrzeugen, wenn nötig.";
            ViewData["RegionalAvailability"] = "Im gesamten Limmattal bieten wir neben der Umzugsreinigung auch <strong>Büroreinigungen für die vielen Gewerbegebiete in Schlieren und Dietikon, Bauschlussreinigung, Fensterreinigung sowie Abo-Reinigungen für Treppenhäuser und Geschäftsräume</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden im Limmattal";
            ViewData["Gemeinden"] = "Aesch:8904,Birmensdorf:8903,Dietikon:8953,Geroldswil:8954,Oberengstringen:8102,Oetwil an der Limmat:8955,Schlieren:8952,Uitikon:8142,Unterengstringen:8103,Urdorf:8902,Weiningen:8104";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-hinwil")]
        public IActionResult UmzugsreinigungBezirkHinwil()
        {
            ViewData["BezirkName"] = "Bezirk Hinwil";
            ViewData["BezirkBadgeLabel"] = "Bezirk Hinwil";
            ViewData["H1"] = "Umzugsreinigung Bezirk Hinwil — Endreinigung mit Abnahmegarantie im Oberland";
            ViewData["H2"] = "Reinigungsfirma im Zürcher Oberland — vom Bachtel bis zum Greifensee";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Hinwil";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Hinwil – im ganzen Zürcher Oberland";
            ViewData["ExamplePlz"] = "8340";
            ViewData["Title"] = "Umzugsreinigung Bezirk Hinwil | Zürcher Oberland | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Hinwil — Endreinigung mit Abnahmegarantie im Zürcher Oberland. Wetzikon, Hinwil, Rüti, Bubikon. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-hinwil";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-hinwil-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Hinwil";
            ViewData["BoundaryPhrase"] = "vom Greifensee bis zum Schnebelhorn";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Hinwil</strong> spannt sich <strong>vom Greifensee bis zum Schnebelhorn</strong>, dem höchsten Punkt im Kanton. Wir reinigen in jeder Gemeinde des Zürcher Oberlands.";
            ViewData["LogisticsParagraph"] = "Im Bezirk Hinwil betreuen wir Liegenschaften von <strong>regionalen Verwaltungen in Wetzikon und Rüti</strong> sowie viele privat geführte Häuser in den ländlicheren Gemeinden Bauma, Wald und Fischenthal. Logistisch sind die Anfahrten ins Tösstal länger, dafür sind die Parksituationen einfach: In Wetzikon-Zentrum brauchen wir Anwohner-Bewilligungen für die blaue Zone, in den meisten anderen Gemeinden können wir direkt vor dem Haus halten. Bei Einfamilienhäusern mit Garagen und Carports planen wir die Reinigungsroute so, dass wir Aussenbereiche wie Terrasse und Cheminée-Abdeckung effizient mitnehmen.";
            ViewData["RegionalAvailability"] = "In der ganzen Region Oberland bieten wir neben der Umzugsreinigung auch <strong>Fenster- und Storenreinigung, Hochdruckreinigung von Terrassen und Carports, Spezialreinigungen sowie Abo-Reinigungen für Mehrfamilienhäuser und Praxen</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Zürcher Oberlands";
            ViewData["Gemeinden"] = "Bäretswil:8344,Bubikon:8608,Dürnten:8635,Fischenthal:8497,Gossau:8625,Grüningen:8627,Hinwil:8340,Rüti:8630,Seegräben:8607,Wald:8636,Wetzikon:8620";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-meilen")]
        public IActionResult UmzugsreinigungBezirkMeilen()
        {
            ViewData["BezirkName"] = "Bezirk Meilen";
            ViewData["BezirkBadgeLabel"] = "Bezirk Meilen";
            ViewData["H1"] = "Umzugsreinigung Bezirk Meilen — Endreinigung mit Abnahmegarantie an der Goldküste";
            ViewData["H2"] = "Reinigungsfirma an der Goldküste — Premium-Reinigung für Premium-Lagen";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Meilen";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Meilen – an der ganzen Goldküste";
            ViewData["ExamplePlz"] = "8706";
            ViewData["Title"] = "Umzugsreinigung Bezirk Meilen | Goldküste | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Meilen — Endreinigung mit Abnahmegarantie an der Goldküste. Küsnacht, Zollikon, Meilen, Stäfa. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-meilen";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-meilen-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Meilen";
            ViewData["BoundaryPhrase"] = "von Zollikon bis Stäfa, am rechten Zürichseeufer";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Meilen</strong> deckt die gesamte Goldküste ab — <strong>von Zollikon bis Stäfa, am rechten Zürichseeufer</strong>. Wir sind die Reinigungsfirma für anspruchsvolle Übergaben.";
            ViewData["LogisticsParagraph"] = "An der Goldküste haben wir es mit <strong>Premium-Verwaltungen wie Wincasa, Livit, Privera</strong> sowie spezialisierten Liegenschaftsverwaltern zu tun, die hochwertige Objekte mit hohen Standards betreuen. Logistisch sind viele Liegenschaften am Hang — schmale Privatzufahrten in Küsnacht-Itschnach, Zumikon und Erlenbach, Tiefgaragen mit Höhenlimiten und teilweise Wege, die wir mit Reinigungswagen nur teilweise befahren können. Wir planen das Equipment entsprechend und bringen leichte, manövrierbare Geräte mit.";
            ViewData["RegionalAvailability"] = "An der ganzen Goldküste bieten wir neben der Umzugsreinigung auch <strong>Premium-Fensterreinigung mit Aussicht-Garantie für bodentiefe Seesicht-Fenster, Spezialreinigung für Marmor und Naturstein, Polster- und Teppichreinigung sowie Abo-Reinigung für Villen und Apartments</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen 11 Gemeinden an der Goldküste";
            ViewData["Gemeinden"] = "Erlenbach:8703,Herrliberg:8704,Hombrechtikon:8634,Küsnacht:8700,Männedorf:8708,Meilen:8706,Oetwil am See:8618,Stäfa:8712,Uetikon am See:8707,Zollikon:8702,Zumikon:8126";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-pfaeffikon")]
        public IActionResult UmzugsreinigungBezirkPfaeffikon()
        {
            ViewData["BezirkName"] = "Bezirk Pfäffikon";
            ViewData["BezirkBadgeLabel"] = "Bezirk Pfäffikon";
            ViewData["H1"] = "Umzugsreinigung Bezirk Pfäffikon — Endreinigung mit Abnahmegarantie";
            ViewData["H2"] = "Reinigungsfirma im Bezirk Pfäffikon — von Illnau-Effretikon bis Fehraltorf";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Pfäffikon";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Pfäffikon – am Pfäffikersee und im Tösstal";
            ViewData["ExamplePlz"] = "8330";
            ViewData["Title"] = "Umzugsreinigung Bezirk Pfäffikon | Abnahmegarantie | Fixpreis";
            ViewData["Description"] = "Umzugsreinigung Bezirk Pfäffikon — Endreinigung mit Abnahmegarantie. Illnau-Effretikon, Pfäffikon, Fehraltorf, Hittnau. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-pfaeffikon";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-pfaeffikon-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Pfäffikon";
            ViewData["BoundaryPhrase"] = "von Illnau-Effretikon bis Sternenberg im Tösstal";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Pfäffikon</strong> deckt die ganze Region ab — <strong>von Illnau-Effretikon bis Sternenberg im Tösstal</strong>. Wir sind in jeder der zwölf Gemeinden regelmässig im Einsatz.";
            ViewData["LogisticsParagraph"] = "Im Bezirk Pfäffikon arbeiten wir mit <strong>regionalen Hausverwaltungen in Pfäffikon und Illnau-Effretikon</strong> sowie vielen kleineren privaten Vermietern in den Tösstal-Gemeinden Bauma, Wila und Sternenberg. Logistisch ist Illnau-Effretikon mit seinen Wohnsiedlungen rund um den Bahnhof gut erschlossen, dort koordinieren wir Anwohner-Bewilligungen und Liftreservationen. In den Tösstal-Dörfern sind die Zufahrten oft eng, aber die Parksituation entspannt — wir können meist direkt am Hauseingang halten.";
            ViewData["RegionalAvailability"] = "In der ganzen Region Pfäffikon bieten wir neben der Umzugsreinigung auch <strong>Fensterreinigung, Storenreinigung, Bauschlussreinigung sowie Abo-Reinigungen für Treppenhäuser und gewerbliche Liegenschaften</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden im Bezirk Pfäffikon";
            ViewData["Gemeinden"] = "Bauma:8494,Fehraltorf:8320,Hittnau:8335,Illnau-Effretikon:8307,Kyburg:8314,Lindau:8315,Pfäffikon:8330,Russikon:8332,Sternenberg:8499,Weisslingen:8484,Wila:8492,Wildberg:8489";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-uster")]
        public IActionResult UmzugsreinigungBezirkUster()
        {
            ViewData["BezirkName"] = "Bezirk Uster";
            ViewData["BezirkBadgeLabel"] = "Bezirk Uster";
            ViewData["H1"] = "Umzugsreinigung Bezirk Uster — Endreinigung mit Abnahmegarantie am Greifensee";
            ViewData["H2"] = "Reinigungsfirma zwischen Greifensee und Pfäffikersee — Bezirk Uster";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Uster";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Uster – im ganzen Greifensee-Gebiet";
            ViewData["ExamplePlz"] = "8610";
            ViewData["Title"] = "Umzugsreinigung Bezirk Uster | Greifensee-Region | Abnahmegarantie";
            ViewData["Description"] = "Umzugsreinigung Bezirk Uster — Endreinigung mit Abnahmegarantie. Uster, Dübendorf, Volketswil, Greifensee. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-uster";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-uster-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Uster";
            ViewData["BoundaryPhrase"] = "von Dübendorf am Glattufer bis Mönchaltorf am Greifensee";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Uster</strong> deckt die gesamte Greifensee-Region ab — <strong>von Dübendorf am Glattufer bis Mönchaltorf am Greifensee</strong>.";
            ViewData["LogisticsParagraph"] = "Der Bezirk Uster ist einer der bevölkerungsreichsten im Kanton — entsprechend professionell sind die Verwaltungen aufgestellt. <strong>Mobimo, Allreal, Pensimo</strong> und grosse regionale Verwaltungen betreuen die Wohnüberbauungen rund um Dübendorf, Volketswil und Uster. Logistisch sind Anwohnerparkkarten und Liftreservationen in den Hochhaus-Überbauungen Standard. In den Seegemeinden Greifensee, Maur und Egg sind die Zufahrten oft schmaler, dafür entspannter beim Parken — meist direkt am Hauseingang.";
            ViewData["RegionalAvailability"] = "In der ganzen Region Uster bieten wir neben der Umzugsreinigung auch <strong>Büroreinigungen für die zahlreichen Gewerbegebiete in Dübendorf und Volketswil, Fensterreinigung mit Seeblick-Garantie, Hochdruckreinigung sowie Abo-Reinigungen</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden im Bezirk Uster";
            ViewData["Gemeinden"] = "Dübendorf:8600,Egg:8132,Fällanden:8117,Greifensee:8606,Maur:8124,Mönchaltorf:8617,Schwerzenbach:8603,Uster:8610,Volketswil:8604,Wangen-Brüttisellen:8306";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-winterthur")]
        public IActionResult UmzugsreinigungBezirkWinterthur()
        {
            ViewData["BezirkName"] = "Bezirk Winterthur";
            ViewData["BezirkBadgeLabel"] = "Bezirk Winterthur";
            ViewData["H1"] = "Umzugsreinigung Bezirk Winterthur — Endreinigung mit Abnahmegarantie";
            ViewData["H2"] = "Reinigungsfirma in der Region Winterthur — von der Altstadt bis ins Tösstal";
            ViewData["HyperLocalH2"] = "Ihr Partner für Reinigungen in allen Gemeinden des Bezirks Winterthur";
            ViewData["LocalValueProp"] = "Umzugsreinigung Bezirk Winterthur – Stadt und Umland";
            ViewData["ExamplePlz"] = "8400";
            ViewData["Title"] = "Umzugsreinigung Bezirk Winterthur | Abnahmegarantie | Fixpreis";
            ViewData["Description"] = "Umzugsreinigung Bezirk Winterthur — Endreinigung mit Abnahmegarantie in Stadt und Umland. Altstadt, Töss, Seen, Wülflingen. Reinigungsfirma vor Ort.";
            ViewData["Canonical"] = "https://zurich-endreinigung.ch/umzugsreinigung-bezirk-winterthur";
            ViewData["HeroImage"] = "/img/bezirke/umzugsreinigung-bezirk-winterthur-zuerich-hero.avif";
            ViewData["KeywordRegion"] = "Bezirk Winterthur";
            ViewData["BoundaryPhrase"] = "von Altikon bis Wiesendangen, von Brütten bis Turbenthal";
            ViewData["HeroIntroLine"] = "Die <strong>Umzugsreinigung Bezirk Winterthur</strong> deckt jede Gemeinde der Region ab — <strong>von Altikon bis Wiesendangen, von Brütten bis Turbenthal</strong>. Wir sind sowohl in der Winterthurer Altstadt wie auch im Tösstal regelmässig im Einsatz.";
            ViewData["LogisticsParagraph"] = "In der Stadt Winterthur kennen wir die grossen Verwaltungen — <strong>Allreal, Mobimo, Wincasa</strong> sowie die regionalen Hausverwaltungen mit Sitz in Winterthur — und ihre Übergabe-Standards. Logistisch ist die Altstadt mit der Fussgänger-Zone und schmalen Gassen anspruchsvoll: Wir brauchen Tagesbewilligungen für die Anlieferung in der blauen Zone, planen Belade-Fenster früh am Morgen und nutzen für Innenstadt-Aufträge kleinere Servicefahrzeuge. In den Aussenquartieren Seen, Wülflingen und Veltheim und in den Umlandgemeinden Brütten, Pfungen und Neftenbach sind die Parksituationen unkompliziert.";
            ViewData["RegionalAvailability"] = "In der gesamten Region Winterthur bieten wir neben der Umzugsreinigung auch <strong>Fenster- und Abo-Reinigungen, Büroreinigungen für die vielen Gewerbeflächen in Neuhegi und Grüze, Bauschlussreinigung sowie Spezialreinigungen für die zahlreichen Loft-Wohnungen in umgebauten Industriegebäuden</strong> an.";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in Winterthur und allen umliegenden Gemeinden";
            ViewData["Gemeinden"] = "Brütten:8311,Dättlikon:8421,Dinhard:8474,Elgg:8353,Elsau:8352,Hagenbuch:8523,Hettlingen:8442,Neftenbach:8413,Pfungen:8422,Rickenbach:8545,Schlatt:8418,Seuzach:8472,Turbenthal:8488,Wiesendangen:8542,Winterthur:8400,Zell:8486";
            return View("UmzugsreinigungBezirk");
        }

        [Route("reinigung-zuerich")]
        public IActionResult ReinigungZuerichRedirect()
        {
            return RedirectPermanent("/umzugsreinigung-stadt-zuerich");
        }

        [Route("reinigung-stadt-zuerich")]
        public IActionResult ReinigungStadtZuerichRedirect() => RedirectPermanent("/umzugsreinigung-stadt-zuerich");

        [Route("reinigung-affoltern")]
        public IActionResult ReinigungAffolternRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-affoltern");

        [Route("reinigung-andelfingen")]
        public IActionResult ReinigungAndelfingenRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-andelfingen");

        [Route("reinigung-buelach")]
        public IActionResult ReinigungBuelachRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-buelach");

        [Route("reinigung-dielsdorf")]
        public IActionResult ReinigungDielsdorfRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-dielsdorf");

        [Route("reinigung-dietikon")]
        public IActionResult ReinigungDietikonRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-dietikon");

        [Route("reinigung-hinwil")]
        public IActionResult ReinigungHinwilRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-hinwil");

        [Route("reinigung-meilen")]
        public IActionResult ReinigungMeilenRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-meilen");

        [Route("reinigung-pfaeffikon")]
        public IActionResult ReinigungPfaeffikonRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-pfaeffikon");

        [Route("reinigung-uster")]
        public IActionResult ReinigungUsterRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-uster");

        [Route("reinigung-winterthur")]
        public IActionResult ReinigungWinterthurRedirect() => RedirectPermanent("/umzugsreinigung-bezirk-winterthur");

        [Route("umzugsreinigung-bezirk-horgen")]
        public IActionResult UmzugsreinigungBezirkHorgen() => View("BezirkHorgen");

        [Route("abgabegarantie")]
        public IActionResult AbgabegarantieRedirect() => RedirectPermanent("/abnahmegarantie");

        [Route("polster-reinigung")]
        public IActionResult PolsterReinigungRedirect() => RedirectPermanent("/polsterreinigung");

        [Route("reinigungsdienste-zuerich")]
        public IActionResult ReinigungsdiensteRedirect() => RedirectPermanent("/");

        [Route("booking")]
        public IActionResult BookingRedirect() => RedirectPermanent("/calculator");

        [Route("event-reinigung")]
        public IActionResult EventReinigung()
        {
            return View();
        }

        [Route("boot-reinigung")]
        public IActionResult BootReinigung()
        {
            return View();
        }

        [Route("hochdruckreinigung-zuerich")]
        public IActionResult Hochdruckreinigung()
        {
            return View();
        }

        [Route("services/hochdruckreinigung")]
        public IActionResult HochdruckreinigungRedirect() => RedirectPermanent("/hochdruckreinigung-zuerich");

        [Route("spezialreinigung")]
        public IActionResult Spezialreinigung()
        {
            return View();
        }

        [Route("gewerbereinigung")]
        public IActionResult Gewerbereinigung()
        {
            return View();
        }

        [Route("standorte")]
        public IActionResult Standorte()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
