
using CineNova.Data;
using CineNova.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineNova.Controllers;

public class CategoriasController : Controller
{
    private readonly CineNovaContext _context;

    public CategoriasController(CineNovaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _context.Categorias
            .Include(c => c.Peliculas)
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(categorias);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Peliculas)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    public IActionResult Create()
    {
        return View(new Categoria());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        categoria.Nombre = categoria.Nombre?.Trim() ?? string.Empty;

        bool existe = await _context.Categorias.AnyAsync(c =>
            c.Nombre.ToLower() == categoria.Nombre.ToLower());

        if (existe)
        {
            ModelState.AddModelError(
                nameof(categoria.Nombre),
                "Ya existe una categoría con ese nombre."
            );
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "La categoría se registró correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(categoria.Nombre),
                    "No se pudo registrar la categoría. Comprueba que el nombre no esté repetido."
                );
            }
        }

        return View(categoria);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Categoria categoria)
    {
        if (id != categoria.Id)
            return NotFound();

        var categoriaActual = await _context.Categorias.FindAsync(id);

        if (categoriaActual == null)
            return NotFound();

        categoria.Nombre = categoria.Nombre?.Trim() ?? string.Empty;

        bool existe = await _context.Categorias.AnyAsync(c =>
            c.Id != categoria.Id &&
            c.Nombre.ToLower() == categoria.Nombre.ToLower());

        if (existe)
        {
            ModelState.AddModelError(
                nameof(categoria.Nombre),
                "Ya existe otra categoría con ese nombre."
            );
        }

        if (ModelState.IsValid)
        {
            try
            {
                categoriaActual.Nombre = categoria.Nombre;
                categoriaActual.Descripcion = categoria.Descripcion;
                categoriaActual.Estado = categoria.Estado;

                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "La categoría se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    nameof(categoria.Nombre),
                    "No se pudo actualizar la categoría. Comprueba que el nombre no esté repetido."
                );
            }
        }

        return View(categoria);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Peliculas)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Peliculas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
        {
            TempData["Error"] = "La categoría que intentas eliminar no existe.";
            return RedirectToAction(nameof(Index));
        }

        if (categoria.Peliculas.Any())
        {
            TempData["Error"] =
                "No puedes eliminar esta categoría porque tiene películas asociadas. Primero cambia la categoría de esas películas o elimínalas.";

            return RedirectToAction(nameof(Index));
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La categoría se eliminó correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
