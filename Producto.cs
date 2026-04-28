using System.ComponentModel.DataAnnotations;

namespace RANCHO_TACO.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        public bool MasVendido { get; set; }

        // Campo opcional para imagen
        public string? ImagenUrl { get; set; }
    }
}