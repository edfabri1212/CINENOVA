
using CineNova.Data;
using CineNova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CineNova.Controllers;

public class PeliculasController : Controller
{
    private readonly CineNovaContext _context;

    public PeliculasController(CineNovaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? buscar, int? categoriaId)
    {
        var query = _context.Peliculas
            .Include(p => p.Categoria)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            query = query.Where(p =>
                EF.Functions.ILike(p.Titulo, $"%{buscar}%"));
        }

        if (categoriaId.HasValue)
        {
            query = query.Where(p => p.CategoriaId == categoriaId.Value);
        }

        ViewBag.Buscar = buscar;
        ViewBag.CategoriaId = categoriaId;
        ViewBag.Categorias = new SelectList(
            await _context.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync(),
            "Id",
            "Nombre",
            categoriaId
        );

        return View(await query.OrderBy(p => p.Titulo).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var pelicula = await _context.Peliculas
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula == null)
            return NotFound();

        return View(pelicula);
    }

    public async Task<IActionResult> Create()
    {
        await CargarCategorias();
        return View(new Pelicula());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pelicula pelicula)
    {
        pelicula.Titulo = pelicula.Titulo?.Trim() ?? string.Empty;

        bool categoriaValida = await _context.Categorias.AnyAsync(c =>
            c.Id == pelicula.CategoriaId && c.Estado);

        if (!categoriaValida)
        {
            ModelState.AddModelError(
                nameof(pelicula.CategoriaId),
                "Selecciona una categoría activa y válida."
            );
        }

        if (ModelState.IsValid)
        {
            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "La película se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategorias(pelicula.CategoriaId);
        return View(pelicula);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var pelicula = await _context.Peliculas.FindAsync(id);

        if (pelicula == null)
            return NotFound();

        await CargarCategorias(pelicula.CategoriaId);
        return View(pelicula);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pelicula pelicula)
    {
        if (id != pelicula.Id)
            return NotFound();

        var peliculaActual = await _context.Peliculas.FindAsync(id);

        if (peliculaActual == null)
            return NotFound();

        pelicula.Titulo = pelicula.Titulo?.Trim() ?? string.Empty;

        bool categoriaValida = await _context.Categorias.AnyAsync(c =>
            c.Id == pelicula.CategoriaId &&
            (c.Estado || c.Id == peliculaActual.CategoriaId));

        if (!categoriaValida)
        {
            ModelState.AddModelError(
                nameof(pelicula.CategoriaId),
                "Selecciona una categoría válida."
            );
        }

        if (ModelState.IsValid)
        {
            peliculaActual.Titulo = pelicula.Titulo;
            peliculaActual.Descripcion = pelicula.Descripcion;
            peliculaActual.Anio = pelicula.Anio;
            peliculaActual.Duracion = pelicula.Duracion;
            peliculaActual.Clasificacion = pelicula.Clasificacion;
            peliculaActual.ImagenUrl = pelicula.ImagenUrl;
            peliculaActual.VideoUrl = pelicula.VideoUrl;
            peliculaActual.CategoriaId = pelicula.CategoriaId;
            peliculaActual.Estado = pelicula.Estado;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "La película se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategorias(pelicula.CategoriaId);
        return View(pelicula);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var pelicula = await _context.Peliculas
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula == null)
            return NotFound();

        return View(pelicula);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pelicula = await _context.Peliculas.FindAsync(id);

        if (pelicula == null)
        {
            TempData["Error"] = "La película que intentas eliminar no existe.";
            return RedirectToAction(nameof(Index));
        }

        _context.Peliculas.Remove(pelicula);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La película se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCategorias(int? seleccionada = null)
    {
        var categorias = await _context.Categorias
            .Where(c => c.Estado || c.Id == seleccionada)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.CategoriaId = new SelectList(
            categorias,
            "Id",
            "Nombre",
            seleccionada
        );
    }
}
