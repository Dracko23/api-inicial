
using ApiInicial.DTOs;

namespace ApiInicial.Services;

public interface IProductoService
{
    Task<List<ProductoResponseDto>> ObtenerTodosAsync();
    Task<ProductoResponseDto?> BuscarProductoPorIdAsync(int id);
    Task<ProductoResponseDto> CrearProductoAsync(CrearProductoDto producto);
    Task<ProductoResponseDto?> ActualizarProductoAsync(int id, ActualizarProductoDto producto);
    Task<bool> EliminarAsync(int id);
    Task<bool> ExisteCategoriaAsync(int categoriaId);
}