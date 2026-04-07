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
