using System.ComponentModel.DataAnnotations;

namespace ApiInicial.DTOs;

public class CrearProductoDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 99999999.99)]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public bool Estado { get; set; } = true;

    [Range(1, int.MaxValue)]
    public int CategoriaId { get; set; }
}
