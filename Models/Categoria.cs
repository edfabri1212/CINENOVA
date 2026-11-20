using System.ComponentModel.DataAnnotations;

namespace CineNova.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre de la categoría")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Activa")]
    public bool Estado { get; set; } = true;

    public ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
