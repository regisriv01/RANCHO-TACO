using Microsoft.AspNetCore.Mvc;
using RANCHO_TACO.Models;
using RanchoTaco.Data;
using System.Diagnostics;
using System.Linq;

namespace RANCHO_TACO.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Inyección del contexto
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // PÁGINA PRINCIPAL (Index)
        // ============================================
        public IActionResult Index()
        {
            // SEED (solo si no hay productos)
            if (!_context.Productos.Any())
            {
                _context.Productos.AddRange(

                    // TACOS (más vendidos)
                    new Producto { Nombre = "Tacos bistec (maíz)", Precio = 100, Categoria = "Tacos", ImagenUrl = "/images/productos/tacosbistecmaiz.jpg", MasVendido = true },
                    new Producto { Nombre = "Tacos pastor (maíz)", Precio = 100, Categoria = "Tacos", ImagenUrl = "/images/productos/tacostrompomaiz.jpg", MasVendido = true },
                    new Producto { Nombre = "Tacos mixtos (maíz)", Precio = 100, Categoria = "Tacos", ImagenUrl = "/images/productos/tacosmixtomaiz.jpg", MasVendido = true },

                    // TACOS HARINA
                    new Producto { Nombre = "Tacos bistec (harina)", Precio = 110, Categoria = "Tacos", ImagenUrl = "/images/productos/tacosbistecharina.jpg" },
                    new Producto { Nombre = "Tacos pastor (harina)", Precio = 110, Categoria = "Tacos", ImagenUrl = "/images/productos/tacostrompoharina.jpg" },
                    new Producto { Nombre = "Tacos mixtos (harina)", Precio = 110, Categoria = "Tacos", ImagenUrl = "/images/productos/tacosmixtoharina.jpg" },

                    // SINCRONIZADAS
                    new Producto { Nombre = "Sincronizada bistec", Precio = 130, Categoria = "Sincronizadas", ImagenUrl = "/images/productos/sincronizadabistec.jpg" },
                    new Producto { Nombre = "Sincronizada pastor", Precio = 130, Categoria = "Sincronizadas", ImagenUrl = "/images/productos/sincronizadatrompo.jpg" },
                    new Producto { Nombre = "Sincronizada mixta", Precio = 130, Categoria = "Sincronizadas", ImagenUrl = "/images/productos/sincronizadamixta.jpg" },

                    // ESPADAS
                    new Producto { Nombre = "Espadas bistec", Precio = 30, Categoria = "Espadas", ImagenUrl = "/images/productos/espadasbistec.jpg" },
                    new Producto { Nombre = "Espadas pastor", Precio = 30, Categoria = "Espadas", ImagenUrl = "/images/productos/espadastrompo.jpg" },
                    new Producto { Nombre = "Espadas mixtas", Precio = 30, Categoria = "Espadas", ImagenUrl = "/images/productos/espadasmixto.jpg" },

                    // GRINGAS
                    new Producto { Nombre = "Gringas", Precio = 120, Categoria = "Gringas", ImagenUrl = "/images/productos/gringas.jpg" },

                    // TORTAS
                    new Producto { Nombre = "Torta bistec", Precio = 55, Categoria = "Tortas", ImagenUrl = "/images/productos/tortabistec.jpg" },
                    new Producto { Nombre = "Torta pastor", Precio = 55, Categoria = "Tortas", ImagenUrl = "/images/productos/tortatrompo.jpg" },
                    new Producto { Nombre = "Torta mixta", Precio = 55, Categoria = "Tortas", ImagenUrl = "/images/productos/tortamixta.jpg" },

                    // PAPAS
                    new Producto { Nombre = "Papa bistec", Precio = 125, Categoria = "Papas", ImagenUrl = "/images/productos/papabistec.jpg" },
                    new Producto { Nombre = "Papa pastor", Precio = 125, Categoria = "Papas", ImagenUrl = "/images/productos/papatrompo.jpg" },
                    new Producto { Nombre = "Papa mixta", Precio = 125, Categoria = "Papas", ImagenUrl = "/images/productos/papamixta.jpg" }
                );

                _context.SaveChanges();
            }

            // PRODUCTOS MÁS VENDIDOS
            var productos = _context.Productos
                .Where(p => p.MasVendido)
                .ToList();

            // RESEÑAS
            var resenas = _context.Resenas != null
                ? _context.Resenas
                    .OrderByDescending(r => r.Id)
                    .Take(5)
                    .ToList()
                : new List<Resena>();

            ViewBag.Resenas = resenas;

            return View(productos);
        }

        // ============================================
        // MENÚ COMPLETO
        // ============================================
        public IActionResult Menu()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        // ============================================
        // ACERCA DE (NUEVO)
        // ============================================
        public IActionResult Acerca()
        {
            return View();
        }

        // ============================================
        // MANEJO DE ERRORES
        // ============================================
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}