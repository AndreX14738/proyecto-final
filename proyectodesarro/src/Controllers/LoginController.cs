using Microsoft.AspNetCore.Mvc;
using proyectodesarro.Helpers;

namespace proyectodesarro.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, string contrasena)
        {
            if (UsuarioHelper.ValidarUsuario(nombre, contrasena))
            {
                // Redirigir a la lista de estudiantes si el login es válido
                return RedirectToAction("Index", "Estudiantes");
            }

            ViewBag.Mensaje = "Credenciales incorrectas";
            return View();
        }

        public IActionResult CerrarSesion()
        {
            // Aquí podrías agregar cualquier limpieza de sesión necesaria
            return RedirectToAction("Index", "Login");
        }
    }
}
