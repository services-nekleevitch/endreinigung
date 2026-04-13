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

        [Route("praxisreinigung")]
        public IActionResult Praxisreinigung()
        {
            return View();
        }

        [Route("baureinigung")]
        public IActionResult Baureinigung()
        {
            return View();
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
            ViewData["H1"] = "Endreinigung im Bezirk Affoltern — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Affoltern";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Affoltern";
            ViewData["ExamplePlz"] = "8910";
            ViewData["Title"] = "Endreinigung Bezirk Affoltern | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Affoltern — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-affoltern";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Affoltern.";
            ViewData["Gemeinden"] = "Affoltern am Albis:8910,Bonstetten:8906,Hausen am Albis:8915,Hedingen:8908,Kappel am Albis:8926,Knonau:8934,Maschwanden:8933,Mettmenstetten:8932,Obfelden:8912,Ottenbach:8913,Rifferswil:8911,Stallikon:8143,Wettswil am Albis:8907,Aeugst am Albis:8914";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-andelfingen")]
        public IActionResult UmzugsreinigungBezirkAndelfingen()
        {
            ViewData["BezirkName"] = "Bezirk Andelfingen";
            ViewData["BezirkBadgeLabel"] = "Bezirk Andelfingen";
            ViewData["H1"] = "Endreinigung im Bezirk Andelfingen — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Andelfingen";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Andelfingen";
            ViewData["ExamplePlz"] = "8450";
            ViewData["Title"] = "Endreinigung Bezirk Andelfingen | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Andelfingen — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-andelfingen";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Andelfingen.";
            ViewData["Gemeinden"] = "Andelfingen:8450,Adlikon:8452,Benken:8463,Berg am Irchel:8415,Buch am Irchel:8414,Dachsen:8447,Dorf:8458,Feuerthalen:8245,Flaach:8416,Flurlingen:8247,Henggart:8444,Humlikon:8457,Kleinandelfingen:8451,Laufen-Uhwiesen:8248,Marthalen:8460,Oberstammheim:8477,Ossingen:8475,Rheinau:8462,Stammheim:8476,Thalheim an der Thur:8478,Trüllikon:8466,Truttikon:8467,Volken:8459";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-buelach")]
        public IActionResult UmzugsreinigungBezirkBuelach()
        {
            ViewData["BezirkName"] = "Bezirk Bülach";
            ViewData["BezirkBadgeLabel"] = "Bezirk Bülach";
            ViewData["H1"] = "Endreinigung im Bezirk Bülach — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Bülach";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Bülach";
            ViewData["ExamplePlz"] = "8180";
            ViewData["Title"] = "Endreinigung Bezirk Bülach | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Bülach — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-buelach";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Bülach.";
            ViewData["Gemeinden"] = "Bülach:8180,Bachenbülach:8184,Bassersdorf:8303,Dietlikon:8305,Eglisau:8193,Embrach:8424,Freienstein-Teufen:8427,Glattfelden:8192,Hochfelden:8182,Höri:8181,Kloten:8302,Lufingen:8426,Nürensdorf:8309,Oberembrach:8425,Opfikon:8152,Rafz:8197,Rorbas:8427,Wallisellen:8304,Wasterkingen:8195,Wil:8196,Winkel:8185";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-dielsdorf")]
        public IActionResult UmzugsreinigungBezirkDielsdorf()
        {
            ViewData["BezirkName"] = "Bezirk Dielsdorf";
            ViewData["BezirkBadgeLabel"] = "Bezirk Dielsdorf";
            ViewData["H1"] = "Endreinigung im Bezirk Dielsdorf — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Dielsdorf";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Dielsdorf";
            ViewData["ExamplePlz"] = "8157";
            ViewData["Title"] = "Endreinigung Bezirk Dielsdorf | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Dielsdorf — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-dielsdorf";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Dielsdorf.";
            ViewData["Gemeinden"] = "Dielsdorf:8157,Bachs:8164,Buchs:8107,Dällikon:8108,Dänikon:8114,Hüttikon:8115,Neerach:8173,Niederglatt:8172,Niederhasli:8155,Niederweningen:8166,Oberglatt:8154,Oberweningen:8165,Otelfingen:8112,Regensberg:8158,Regensdorf:8105,Rümlang:8153,Schleinikon:8167,Schöfflisdorf:8165,Stadel:8174,Steinmaur:8162,Weiach:8187";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-dietikon")]
        public IActionResult UmzugsreinigungBezirkDietikon()
        {
            ViewData["BezirkName"] = "Bezirk Dietikon";
            ViewData["BezirkBadgeLabel"] = "Bezirk Dietikon";
            ViewData["H1"] = "Endreinigung im Bezirk Dietikon — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Dietikon";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Dietikon";
            ViewData["ExamplePlz"] = "8953";
            ViewData["Title"] = "Endreinigung Bezirk Dietikon | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Dietikon — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-dietikon";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Dietikon.";
            ViewData["Gemeinden"] = "Dietikon:8953,Aesch:8904,Birmensdorf:8903,Geroldswil:8954,Oberengstringen:8102,Oetwil an der Limmat:8955,Schlieren:8952,Spreitenbach:8957,Uitikon:8142,Unterengstringen:8103,Urdorf:8902,Weiningen:8104";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-hinwil")]
        public IActionResult UmzugsreinigungBezirkHinwil()
        {
            ViewData["BezirkName"] = "Bezirk Hinwil";
            ViewData["BezirkBadgeLabel"] = "Bezirk Hinwil";
            ViewData["H1"] = "Endreinigung im Bezirk Hinwil — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Hinwil";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Hinwil";
            ViewData["ExamplePlz"] = "8340";
            ViewData["Title"] = "Endreinigung Bezirk Hinwil | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Hinwil — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-hinwil";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Hinwil.";
            ViewData["Gemeinden"] = "Hinwil:8340,Bäretswil:8344,Bubikon:8608,Dürnten:8635,Fischenthal:8497,Gossau:8625,Grüningen:8627,Rüti:8630,Seegräben:8607,Wald:8636,Wetzikon:8620";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-meilen")]
        public IActionResult UmzugsreinigungBezirkMeilen()
        {
            ViewData["BezirkName"] = "Bezirk Meilen";
            ViewData["BezirkBadgeLabel"] = "Bezirk Meilen";
            ViewData["H1"] = "Endreinigung im Bezirk Meilen — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Meilen";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Meilen";
            ViewData["ExamplePlz"] = "8706";
            ViewData["Title"] = "Endreinigung Bezirk Meilen | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Meilen — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-meilen";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Meilen.";
            ViewData["Gemeinden"] = "Meilen:8706,Erlenbach:8703,Herrliberg:8704,Hombrechtikon:8634,Küsnacht:8700,Männedorf:8708,Oetwil am See:8618,Stäfa:8712,Uetikon am See:8707,Zollikon:8702,Zumikon:8126";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-pfaeffikon")]
        public IActionResult UmzugsreinigungBezirkPfaeffikon()
        {
            ViewData["BezirkName"] = "Bezirk Pfäffikon";
            ViewData["BezirkBadgeLabel"] = "Bezirk Pfäffikon";
            ViewData["H1"] = "Endreinigung im Bezirk Pfäffikon — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Pfäffikon";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Pfäffikon";
            ViewData["ExamplePlz"] = "8330";
            ViewData["Title"] = "Endreinigung Bezirk Pfäffikon | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Pfäffikon — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-pfaeffikon";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Pfäffikon.";
            ViewData["Gemeinden"] = "Pfäffikon:8330,Bauma:8494,Fehraltorf:8320,Hittnau:8335,Illnau-Effretikon:8307,Kyburg:8314,Lindau:8315,Russikon:8332,Sternenberg:8499,Weisslingen:8484,Wila:8492,Wildberg:8489";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-uster")]
        public IActionResult UmzugsreinigungBezirkUster()
        {
            ViewData["BezirkName"] = "Bezirk Uster";
            ViewData["BezirkBadgeLabel"] = "Bezirk Uster";
            ViewData["H1"] = "Endreinigung im Bezirk Uster — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Uster";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Uster";
            ViewData["ExamplePlz"] = "8610";
            ViewData["Title"] = "Endreinigung Bezirk Uster | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Uster — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-uster";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Uster.";
            ViewData["Gemeinden"] = "Uster:8610,Dübendorf:8600,Egg:8132,Fällanden:8117,Greifensee:8606,Maur:8124,Mönchaltorf:8617,Schwerzenbach:8603,Volketswil:8604,Wangen-Brüttisellen:8306";
            return View("UmzugsreinigungBezirk");
        }

        [Route("umzugsreinigung-bezirk-winterthur")]
        public IActionResult UmzugsreinigungBezirkWinterthur()
        {
            ViewData["BezirkName"] = "Bezirk Winterthur";
            ViewData["BezirkBadgeLabel"] = "Bezirk Winterthur";
            ViewData["H1"] = "Endreinigung im Bezirk Winterthur — Umzugsreinigung mit 100% Abnahmegarantie";
            ViewData["H2"] = "Ihr Reinigungspartner im Bezirk Winterthur";
            ViewData["LocalValueProp"] = "Lokale Reinigungsprofis im gesamten Bezirk Winterthur";
            ViewData["ExamplePlz"] = "8400";
            ViewData["Title"] = "Endreinigung Bezirk Winterthur | Fixpreis ab CHF 550 | Abnahmegarantie";
            ViewData["Description"] = "Professionelle Endreinigung im Bezirk Winterthur — Fixpreis, persönliche Abnahmebegleitung, 100% Garantie.";
            ViewData["Canonical"] = "https://zuerich-endreinigung.ch/umzugsreinigung-bezirk-winterthur";
            ViewData["GemeindenSubtitle"] = "Wir reinigen in allen Gemeinden des Bezirks Winterthur.";
            ViewData["Gemeinden"] = "Winterthur:8400,Brütten:8311,Dättlikon:8421,Dinhard:8474,Elgg:8353,Elsau:8352,Hettlingen:8442,Neftenbach:8413,Pfungen:8422,Rickenbach:8545,Seuzach:8472,Turbenthal:8488,Wiesendangen:8542,Zell:8468";
            return View("UmzugsreinigungBezirk");
        }

        [Route("reinigung-zuerich")]
        public IActionResult ReinigungZuerichRedirect()
        {
            return RedirectPermanent("/umzugsreinigung-stadt-zuerich");
        }

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
