using Microsoft.AspNetCore.Mvc;
using proyectodesarro.Models;
using proyectodesarro.Helpers;

namespace proyectodesarro.Controllers
{
    public class NotasController : Controller
    {
        public IActionResult Index(int estudianteId)
        {
            var notas = CSVHelper.LeerNotas(estudianteId);
            ViewBag.EstudianteId = estudianteId;
            var estudiante = CSVHelper.LeerEstudiantes().FirstOrDefault(e => e.Id == estudianteId);
            ViewBag.NombreEstudiante = estudiante != null ? $"{estudiante.Nombre} {estudiante.Apellidos}" : "";
            return View(notas);
        }

        public IActionResult Crear(int estudianteId)
        {
            var nota = new Nota 
            { 
                EstudianteId = estudianteId,
                Materia = string.Empty,
                Periodo = string.Empty
            };
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost]
        public IActionResult Crear(Nota nota)
        {
            if (ModelState.IsValid)
            {
                CSVHelper.GuardarNota(nota);
                return RedirectToAction(nameof(Index), new { estudianteId = nota.EstudianteId });
            }
            ViewBag.EstudianteId = nota.EstudianteId;
            return View(nota);
        }

        public IActionResult Editar(int id, int estudianteId)
        {
            var notas = CSVHelper.LeerNotas(estudianteId);
            var nota = notas.FirstOrDefault(n => n.Id == id);
            if (nota == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost]
        public IActionResult Editar(int id, Nota nota)
        {
            if (id != nota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var notas = CSVHelper.LeerNotas(nota.EstudianteId);
                var index = notas.FindIndex(n => n.Id == id);
                if (index != -1)
                {
                    notas[index] = nota;
                    System.IO.File.WriteAllText("notas.csv", string.Empty);
                    foreach (var n in notas)
                    {
                        CSVHelper.GuardarNota(n);
                    }
                    return RedirectToAction(nameof(Index), new { estudianteId = nota.EstudianteId });
                }
            }
            ViewBag.EstudianteId = nota.EstudianteId;
            return View(nota);
        }

        public IActionResult Eliminar(int id, int estudianteId)
        {
            var notas = CSVHelper.LeerNotas(estudianteId);
            var nota = notas.FirstOrDefault(n => n.Id == id);
            if (nota == null)
            {
                return NotFound();
            }
            ViewBag.EstudianteId = estudianteId;
            return View(nota);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id, int estudianteId)
        {
            var notas = CSVHelper.LeerNotas(estudianteId);
            var nota = notas.FirstOrDefault(n => n.Id == id);
            if (nota != null)
            {
                notas.Remove(nota);
                System.IO.File.WriteAllText("notas.csv", string.Empty);
                foreach (var n in notas)
                {
                    CSVHelper.GuardarNota(n);
                }
            }
            return RedirectToAction(nameof(Index), new { estudianteId });
        }
    }
}
