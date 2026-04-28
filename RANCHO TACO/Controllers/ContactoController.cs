using Microsoft.AspNetCore.Mvc;
using RANCHO_TACO.Models;
using RanchoTaco.Data;

namespace RANCHO_TACO.Controllers
{
    public class ContactoController : Controller
    {
        private readonly AppDbContext _context;

        public ContactoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(Resena resena)
        {
            if (ModelState.IsValid)
            {
                _context.Resenas.Add(resena);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View("Index");
        }
    }
}