using System.ComponentModel.DataAnnotations;

namespace RANCHO_TACO.Models
{
    public class Resena
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Comentario { get; set; }

        public int Calificacion { get; set; } // 1 a 5 estrellas
    }
}