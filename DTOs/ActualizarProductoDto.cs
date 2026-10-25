using System.ComponentModel.DataAnnotations;

namespace ApiInicial.DTOs;

public class ActualizarProductoDto
{
    [StringLength(100, MinimumLength = 1)]
    public string? Nombre { get; set; }

    [Range(0, 99999999.99)]
    public decimal? Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }

    public bool? Estado { get; set; }

    [Range(1, int.MaxValue)]
    public int? CategoriaId { get; set; }
}
