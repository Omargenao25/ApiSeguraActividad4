using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSeguraActividad4.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Range(1450, 2100, ErrorMessage = "El año de publicación debe estar entre 1450 y 2100.")]
        public int AnioPublicacion { get; set; }

        [Required(ErrorMessage = "El género es requerido.")]
        [StringLength(50, ErrorMessage = "El género no puede exceder los 50 caracteres.")]
        public string Genero { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "El número de páginas debe ser mayor que cero.")]
        public int NumeroPaginas { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero.")]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El AutorId debe ser mayor que cero.")]
        public int AutorId { get; set; }


        [ForeignKey("AutorId")]
        public Autor? Autor { get; set; }
    }
}