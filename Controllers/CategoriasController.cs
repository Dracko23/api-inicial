using ApiInicial.DTOs;
using ApiInicial.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponseDto>>> ObtenerTodos()
    {
        return await _categoriaService.ObtenerTodosAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaResponseDto>> ObtenerPorId(int id)
    {
        var categoria = await _categoriaService.BuscarPorIdAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaResponseDto>> Crear(CrearCategoriaDto categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nombre))
        {
            return BadRequest(new { mensaje = "El nombre de la categoria es obligatorio." });
        }

        var categoriaCreada = await _categoriaService.CrearAsync(categoria);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = categoriaCreada.Id }, categoriaCreada);
    }
}
