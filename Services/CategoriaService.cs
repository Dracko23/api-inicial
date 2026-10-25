using ApiInicial.Data;
using ApiInicial.DTOs;
using ApiInicial.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInicial.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Id)
            .Select(categoria => new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            })
            .ToListAsync();
    }

    public async Task<CategoriaResponseDto?> BuscarPorIdAsync(int id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(categoria => categoria.Id == id)
            .Select(categoria => new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaResponseDto> CrearAsync(CrearCategoriaDto categoriaDto)
    {
        var categoria = new Categoria
        {
            Nombre = categoriaDto.Nombre.Trim(),
            Descripcion = categoriaDto.Descripcion?.Trim()
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };
    }
}
