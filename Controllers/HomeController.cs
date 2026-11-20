using CineNova.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineNova.Controllers;

public class HomeController : Controller
{
    private readonly CineNovaContext _context;

    public HomeController(CineNovaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Peliculas
            .Include(p => p.Categoria)
            .Where(p => p.Estado && p.Categoria!.Estado)
            .OrderByDescending(p => p.Id)
            .Take(12)
            .ToListAsync();

        ViewBag.TotalPeliculas = await _context.Peliculas.CountAsync();
        ViewBag.TotalCategorias = await _context.Categorias.CountAsync();
        return View(peliculas);
    }

    public IActionResult Privacy() => View();
}
