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
        }

        public IActionResult Detalles(int id)
        {
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }
    }
}
