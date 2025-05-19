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
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Estudiante est)
        {
            CSVHelper.GuardarEstudiante(est);
            return RedirectToAction("Index");
        }
    }
}
