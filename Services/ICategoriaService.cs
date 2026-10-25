using ApiInicial.DTOs;

namespace ApiInicial.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> ObtenerTodosAsync();
    Task<CategoriaResponseDto?> BuscarPorIdAsync(int id);
    Task<CategoriaResponseDto> CrearAsync(CrearCategoriaDto categoria);
}
