using ApiInicial.Data;
using ApiInicial.DTOs;
using ApiInicial.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInicial.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoResponseDto>> ObtenerTodosAsync()
    {
        return await _context.Productos
            .AsNoTracking()
            .OrderBy(producto => producto.Id)
            .Select(producto => new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Estado = producto.Estado,
                CategoriaId = producto.CategoriaId,
                NombreCategoria = producto.Categoria.Nombre
            })
            .ToListAsync();
    }

    public async Task<ProductoResponseDto?> BuscarProductoPorIdAsync(int id)
    {
        return await _context.Productos
            .AsNoTracking()
            .Where(producto => producto.Id == id)
            .Select(producto => new ProductoResponseDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Estado = producto.Estado,
                CategoriaId = producto.CategoriaId,
                NombreCategoria = producto.Categoria.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductoResponseDto> CrearProductoAsync(CrearProductoDto productoDto)
    {
        var producto = new Producto
        {
            Nombre = productoDto.Nombre.Trim(),
            Precio = productoDto.Precio,
            Stock = productoDto.Stock,
            Estado = productoDto.Estado,
            CategoriaId = productoDto.CategoriaId
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return await BuscarProductoPorIdAsync(producto.Id)
            ?? throw new InvalidOperationException("No se pudo recuperar el producto creado.");
    }

    public async Task<ProductoResponseDto?> ActualizarProductoAsync(
        int id,
        ActualizarProductoDto productoDto)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null)
        {
            return null;
        }

        if (productoDto.Nombre is not null)
        {
            producto.Nombre = productoDto.Nombre.Trim();
        }

        if (productoDto.Precio.HasValue)
        {
            producto.Precio = productoDto.Precio.Value;
        }

        if (productoDto.Stock.HasValue)
        {
            producto.Stock = productoDto.Stock.Value;
        }

        if (productoDto.Estado.HasValue)
        {
            producto.Estado = productoDto.Estado.Value;
        }

        if (productoDto.CategoriaId.HasValue)
        {
            producto.CategoriaId = productoDto.CategoriaId.Value;
        }

        await _context.SaveChangesAsync();
        return await BuscarProductoPorIdAsync(id);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is null)
        {
            return false;
        }

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> ExisteCategoriaAsync(int categoriaId)
    {
        return _context.Categorias.AnyAsync(categoria => categoria.Id == categoriaId);
    }
}
