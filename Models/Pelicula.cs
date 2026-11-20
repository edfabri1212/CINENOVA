using System.ComponentModel.DataAnnotations;

namespace CineNova.Models;

public class Pelicula
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000)]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Range(1888, 2100, ErrorMessage = "Introduce un año válido.")]
    [Display(Name = "Año")]
    public int Anio { get; set; } = DateTime.Now.Year;

    [Range(1, 1000, ErrorMessage = "La duración debe ser mayor que cero.")]
    [Display(Name = "Duración (minutos)")]
    public int Duracion { get; set; }

    [StringLength(20)]
    [Display(Name = "Clasificación")]
    public string? Clasificacion { get; set; }

    [StringLength(500)]
    [Display(Name = "URL de la portada")]
    public string? ImagenUrl { get; set; }

    [StringLength(1000)]
    [Display(Name = "URL del video autorizado")]
    public string? VideoUrl { get; set; }

    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    [Display(Name = "Disponible")]
    public bool Estado { get; set; } = true;

    public Categoria? Categoria { get; set; }
}
