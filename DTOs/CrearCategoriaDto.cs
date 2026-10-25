using System.ComponentModel.DataAnnotations;

namespace ApiInicial.DTOs;

public class CrearCategoriaDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Descripcion { get; set; }
}
