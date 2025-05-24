using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectodesarro.Models;
using proyectodesarro.Data;

namespace proyectodesarro.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsistenciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int estudianteId)
        {
            var asistencias = await _context.Asistencias
                .Where(a => a.EstudianteId == estudianteId)
                .ToListAsync();
            ViewBag.EstudianteId = estudianteId;
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id == estudianteId);
            ViewBag.NombreEstudiante = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellidos}" : "";
            return View(asistencias);
        }

        public IActionResult Registrar(int estudianteId)
        {
            var asistencia = new Asistencia 
            { 
                EstudianteId = estudianteId,
                Materia = string.Empty,
                Estado = "Presente",
                Fecha = DateTime.Now
            };
            ViewBag.EstudianteId = estudianteId;
            return View(asistencia);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                asistencia.Fecha = DateTime.Now;
                _context.Add(asistencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { estudianteId = asistencia.EstudianteId });
            }
            ViewBag.EstudianteId = asistencia.EstudianteId;
            return View(asistencia);
        }

        public async Task<IActionResult> Editar(int id, int estudianteId)
        {
            var asistencia = await _context.Asistencias
                .FirstOrDefaultAsync(a => a.Id == id && a.EstudianteId == estudianteId);
            if (asistencia == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(asistencia);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Asistencia asistencia)
        {
            if (id != asistencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistencia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { estudianteId = asistencia.EstudianteId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaExists(asistencia.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            ViewBag.EstudianteId = asistencia.EstudianteId;
            return View(asistencia);
        }

        public async Task<IActionResult> ReporteAsistencia(int estudianteId)
        {
            var asistencias = await _context.Asistencias
                .Where(a => a.EstudianteId == estudianteId)
                .ToListAsync();
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Id == estudianteId);
            ViewBag.EstudianteId = estudianteId;
            ViewBag.NombreEstudiante = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellidos}" : "";

            var reporte = new Dictionary<string, Dictionary<string, int>>();
            foreach (var asistencia in asistencias)
            {
                if (!reporte.ContainsKey(asistencia.Materia))
                {
                    reporte[asistencia.Materia] = new Dictionary<string, int>
                    {
                        { "Presente", 0 },
                        { "Ausente", 0 },
                        { "Tardanza", 0 },
                        { "Justificado", 0 }
                    };
                }
                reporte[asistencia.Materia][asistencia.Estado]++;
            }

            return View(reporte);
        }

        private bool AsistenciaExists(int id)
        {
            return _context.Asistencias.Any(a => a.Id == id);
        }
    }
}
