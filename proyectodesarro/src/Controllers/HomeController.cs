using Microsoft.AspNetCore.Mvc;
using proyectodesarro.Models;
using proyectodesarro.Helpers;

namespace proyectodesarro.Controllers
{
    public class EstudiantesController : Controller
    {
        public IActionResult Index()
        {
            var estudiantes = CSVHelper.LeerEstudiantes();
            return View(estudiantes);
        }

        public IActionResult Crear()
        {
            var estudiante = new Estudiante
            {
                Nombre = string.Empty,
                Apellidos = string.Empty,
                Correo = string.Empty,
                DocumentoIdentidad = string.Empty,
                Estado = "Activo",
                Grado = string.Empty,
                Seccion = string.Empty
            };
            return View(estudiante);
        }

        [HttpPost]
        public IActionResult Crear(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                CSVHelper.GuardarEstudiante(estudiante);
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        public IActionResult Editar(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        [HttpPost]
        public IActionResult Editar(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var estudiantes = CSVHelper.LeerEstudiantes();
                var index = estudiantes.FindIndex(e => e.Id == id);
                if (index != -1)
                {
                    estudiantes[index] = estudiante;
                    System.IO.File.WriteAllText("estudiantes.csv", string.Empty);
                    foreach (var est in estudiantes)
                    {
                        CSVHelper.GuardarEstudiante(est);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(estudiante);
        }

        public IActionResult Eliminar(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            var estudiantes = CSVHelper.LeerEstudiantes();
            var estudiante = estudiantes.FirstOrDefault(e => e.Id == id);
            if (estudiante != null)
            {
                estudiantes.Remove(estudiante);
                System.IO.File.WriteAllText("estudiantes.csv", string.Empty);
                foreach (var est in estudiantes)
                {
                    CSVHelper.GuardarEstudiante(est);
                }
            }
            return RedirectToAction(nameof(Index));
        }        public IActionResult Detalles(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            // Obtener notas del estudiante
            var notas = CSVHelper.LeerNotas(id)
                .OrderBy(n => n.FechaRegistro)
                .ToList();

            ViewBag.PromedioNotas = notas.Any() 
                ? Math.Round(notas.Average(n => (double)n.Valor), 1) 
                : 0;

            // Determinar el rendimiento
            var promedio = ViewBag.PromedioNotas;
            if (promedio >= 18)
            {
                ViewBag.Rendimiento = "Excelente";
                ViewBag.RendimientoClass = "grade-excellent";
            }
            else if (promedio >= 14)
            {
                ViewBag.Rendimiento = "Bueno";
                ViewBag.RendimientoClass = "grade-good";
            }
            else if (promedio >= 11)
            {
                ViewBag.Rendimiento = "Regular";
                ViewBag.RendimientoClass = "grade-regular";
            }
            else
            {
                ViewBag.Rendimiento = "Necesita Mejorar";
                ViewBag.RendimientoClass = "grade-needs-improvement";
            }

            // Calcular asistencia
            var asistencias = CSVHelper.LeerAsistencias(id)
                .ToList();

            var totalDias = (DateTime.Today - estudiante.FechaIngreso).Days + 1;
            var diasAsistidos = asistencias.Count(a => a.Estado == "Presente");
            ViewBag.PorcentajeAsistencia = totalDias > 0 
                ? Math.Round((double)diasAsistidos / totalDias * 100, 1) 
                : 0;

            // Datos para el gráfico
            ViewBag.FechasNotas = notas.Select(n => n.FechaRegistro.ToString("dd/MM/yyyy")).ToList();
            ViewBag.ValoresNotas = notas.Select(n => (double)n.Valor).ToList();

            // Total de materias
            ViewBag.TotalCursos = notas.Select(n => n.Materia).Distinct().Count();

            return View(estudiante);
        }
    }    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Bienvenido";
            
            // Obtener estadísticas
            var estudiantes = CSVHelper.LeerEstudiantes();
            ViewBag.TotalEstudiantes = estudiantes.Count;

            // Calcular asistencia de hoy
            var asistencias = CSVHelper.LeerTodasLasAsistencias()
                .Where(a => a.Fecha.Date == DateTime.Today)
                .Count();
            ViewBag.AsistenciaHoy = asistencias;

            // Calcular promedio general de notas
            var notas = CSVHelper.LeerTodasLasNotas();
            ViewBag.PromedioNotas = notas.Any() 
                ? Math.Round(notas.Average(n => n.Valor), 1) 
                : 0;

            return View();
        }
    }
}
