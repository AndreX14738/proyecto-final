using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectodesarro.Models;
using proyectodesarro.Data;

namespace proyectodesarro.Controllers
{
    public class NotasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int estudianteId)
        {
            var notas = await _context.Notas
                .Where(n => n.EstudianteId == estudianteId)
                .ToListAsync();
            ViewBag.EstudianteId = estudianteId;
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id == estudianteId);
            ViewBag.NombreEstudiante = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellidos}" : "";
            return View(notas);
        }

        public IActionResult Crear(int estudianteId)
        {
            var nota = new Nota 
            { 
                EstudianteId = estudianteId,
                Materia = string.Empty,
                Periodo = string.Empty,
                FechaRegistro = DateTime.Now
            };
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Nota nota)
        {
            if (ModelState.IsValid)
            {
                nota.FechaRegistro = DateTime.Now;
                _context.Add(nota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { estudianteId = nota.EstudianteId });
            }
            ViewBag.EstudianteId = nota.EstudianteId;
            return View(nota);
        }

        public async Task<IActionResult> Editar(int id, int estudianteId)
        {
            var nota = await _context.Notas
                .FirstOrDefaultAsync(n => n.Id == id && n.EstudianteId == estudianteId);
            if (nota == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Nota nota)
        {
            if (id != nota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nota);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { estudianteId = nota.EstudianteId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(nota.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            ViewBag.EstudianteId = nota.EstudianteId;
            return View(nota);
        }

        public async Task<IActionResult> Eliminar(int id, int estudianteId)
        {
            var nota = await _context.Notas
                .FirstOrDefaultAsync(n => n.Id == id && n.EstudianteId == estudianteId);
            if (nota == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminar(int id, int estudianteId)
        {
            var nota = await _context.Notas
                .FirstOrDefaultAsync(n => n.Id == id && n.EstudianteId == estudianteId);
            if (nota != null)
            {
                _context.Notas.Remove(nota);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { estudianteId });
        }

        private bool NotaExists(int id)
        {
            return _context.Notas.Any(n => n.Id == id);
        }
    }
}
