using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiSeguraActividad4.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nacionalidad es requerida.")]
        [StringLength(50, ErrorMessage = "La nacionalidad no puede exceder los 50 caracteres.")]
        public string Nacionalidad { get; set; } = string.Empty;

        [Range(1500, 2100, ErrorMessage = "El año de nacimiento debe estar entre 1500 y 2100.")]
        public int AnioNacimiento { get; set; }



        [JsonIgnore]
        public ICollection<Libro>? Libros { get; set; } = new List<Libro>();
    }
}
