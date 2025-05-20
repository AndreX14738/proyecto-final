using Microsoft.AspNetCore.Mvc;
using proyectodesarro.Models;
using proyectodesarro.Helpers;

namespace proyectodesarro.Controllers
{
    public class AsistenciasController : Controller
    {
        public IActionResult Index(int estudianteId)
        {
            var asistencias = CSVHelper.LeerAsistencias(estudianteId);
            ViewBag.EstudianteId = estudianteId;
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == estudianteId);
            ViewBag.NombreEstudiante = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellidos}" : "";
            return View(asistencias);
        }

        public IActionResult Registrar(int estudianteId)
        {
            var asistencia = new Asistencia 
            { 
                EstudianteId = estudianteId,
                Materia = string.Empty,
                Estado = "Presente"
            };
            ViewBag.EstudianteId = estudianteId;
            return View(asistencia);
        }

        [HttpPost]
        public IActionResult Registrar(Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
                CSVHelper.GuardarAsistencia(asistencia);
                return RedirectToAction(nameof(Index), new { estudianteId = asistencia.EstudianteId });
            }
            ViewBag.EstudianteId = asistencia.EstudianteId;
            return View(asistencia);
        }

        public IActionResult Editar(int id, int estudianteId)
        {
            var asistencias = CSVHelper.LeerAsistencias(estudianteId);
            var asistencia = asistencias.FirstOrDefault(a => a.Id == id);
            if (asistencia == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(asistencia);
        }

        [HttpPost]
        public IActionResult Editar(int id, Asistencia asistencia)
        {
            if (id != asistencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var asistencias = CSVHelper.LeerAsistencias(asistencia.EstudianteId);
                var index = asistencias.FindIndex(a => a.Id == id);
                if (index != -1)
                {
                    asistencias[index] = asistencia;
                    System.IO.File.WriteAllText("asistencias.csv", string.Empty);
                    foreach (var a in asistencias)
                    {
                        CSVHelper.GuardarAsistencia(a);
                    }
                    return RedirectToAction(nameof(Index), new { estudianteId = asistencia.EstudianteId });
                }
            }
            ViewBag.EstudianteId = asistencia.EstudianteId;
            return View(asistencia);
        }

        public IActionResult ReporteAsistencia(int estudianteId)
        {
            var asistencias = CSVHelper.LeerAsistencias(estudianteId);
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == estudianteId);
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
    }
}
