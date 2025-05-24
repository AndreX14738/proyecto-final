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
        {            var estudiante = new Estudiante
            {
                Nombre = string.Empty,
                Apellidos = string.Empty,
                Correo = string.Empty,
                DocumentoIdentidad = string.Empty,
                Estado = "Activo",
                Grado = string.Empty,
                Seccion = string.Empty,
                Materia = string.Empty,
                DocenteId = 0
            };

            // Cargar lista de docentes para el dropdown
            var docentes = CSVHelper.LeerUsuarios();
            ViewBag.Docentes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                docentes, "Id", "Nombre"
            );

            return View(estudiante);
        }

        [HttpPost]
        public IActionResult Crear(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                // Guardar estudiante
                CSVHelper.GuardarEstudiante(estudiante);

                // Crear registro inicial de asistencia
                if (!string.IsNullOrEmpty(estudiante.AsistenciaInicial))
                {
                    var asistencia = new Asistencia
                    {
                        EstudianteId = estudiante.Id,
                        Materia = estudiante.Materia,
                        Estado = estudiante.AsistenciaInicial,
                        Fecha = DateTime.Now
                    };
                    CSVHelper.GuardarAsistencia(asistencia);
                }

                // Crear registro inicial de nota
                if (estudiante.NotaInicial.HasValue)
                {
                    var nota = new Nota
                    {
                        EstudianteId = estudiante.Id,
                        Materia = estudiante.Materia,
                        Valor = estudiante.NotaInicial.Value,
                        Periodo = DateTime.Now.Year + "-" + (DateTime.Now.Month <= 6 ? "1" : "2"),
                        FechaRegistro = DateTime.Now
                    };
                    CSVHelper.GuardarNota(nota);
                }

                return RedirectToAction(nameof(Index));
            }

            // Si llegamos aquí, algo falló
            var docentes = CSVHelper.LeerUsuarios();
            ViewBag.Docentes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                docentes, "Id", "Nombre", estudiante.DocenteId
            );
            return View(estudiante);
        }

        public IActionResult Editar(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            var docentes = CSVHelper.LeerUsuarios();
            ViewBag.Docentes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                docentes, "Id", "Nombre", estudiante.DocenteId
            );
            return View("Crear", estudiante);
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

            var docentes = CSVHelper.LeerUsuarios();
            ViewBag.Docentes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                docentes, "Id", "Nombre", estudiante.DocenteId
            );
            return View("Crear", estudiante);
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
        }

        public IActionResult Detalles(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            // Obtener docente asignado
            var docente = CSVHelper.LeerUsuarios().FirstOrDefault(d => d.Id == estudiante.DocenteId);
            ViewBag.NombreDocente = docente?.Nombre ?? "No asignado";

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
            var asistencias = CSVHelper.LeerAsistencias(id).ToList();
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
    }
}
