using System.ComponentModel.DataAnnotations;

namespace RANCHO_TACO.Models
{
    // Representa un producto del menú dentro del sistema
    public class Producto
    {
        // Identificador único del producto (clave primaria)
        public int Id { get; set; }

        // Nombre del producto (ej: Tacos al pastor)
        [Required]
        public string Nombre { get; set; } = string.Empty;

        // Precio del producto
        [Required]
        public decimal Precio { get; set; }

        // Indica si el producto es de los más vendidos (para mostrar en inicio)
        public bool MasVendido { get; set; }

        // Categoría del producto (Tacos, Tortas, etc.)
        public string Categoria { get; set; } = string.Empty;

        // Ruta de la imagen dentro de wwwroot
        // Ejemplo: /images/productos/tacos.jpg
        public string ImagenUrl { get; set; } = string.Empty;
    }
}