
using ApiInicial.DTOs;
using ApiInicial.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/productos")]
[Route("api/v1/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoResponseDto>>> ObtenerTodos()
    {
        return await _productoService.ObtenerTodosAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoResponseDto>> ObtenerPorId(int id)
    {
        var producto = await _productoService.BuscarProductoPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoResponseDto>> CrearProducto(CrearProductoDto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
        {
            return BadRequest(new { mensaje = "El nombre del producto es obligatorio." });
        }

        if (!await _productoService.ExisteCategoriaAsync(producto.CategoriaId))
        {
            return BadRequest(new { mensaje = "La categoria indicada no existe." });
        }

        var productoCreado = await _productoService.CrearProductoAsync(producto);
        return Created($"/api/productos/{productoCreado.Id}", productoCreado);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ProductoResponseDto>> ActualizarProducto(
        int id,
        ActualizarProductoDto producto)
    {
        if (producto.Nombre is not null && string.IsNullOrWhiteSpace(producto.Nombre))
        {
            return BadRequest(new { mensaje = "El nombre del producto no puede estar vacio." });
        }

        if (producto.Nombre is null
            && producto.Precio is null
            && producto.Stock is null
            && producto.Estado is null
            && producto.CategoriaId is null)
        {
            return BadRequest(new { mensaje = "Debe indicar al menos un campo para actualizar." });
        }

        if (producto.CategoriaId.HasValue
            && !await _productoService.ExisteCategoriaAsync(producto.CategoriaId.Value))
        {
            return BadRequest(new { mensaje = "La categoria indicada no existe." });
        }

        var productoActualizado = await _productoService.ActualizarProductoAsync(id, producto);
        return productoActualizado is null ? NotFound() : Ok(productoActualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var eliminado = await _productoService.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}