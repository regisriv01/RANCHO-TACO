using Microsoft.AspNetCore.Mvc;

namespace RANCHO_TACO.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
