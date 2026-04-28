using Microsoft.AspNetCore.Mvc;

namespace RANCHO_TACO.Controllers
{
    public class PagoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}