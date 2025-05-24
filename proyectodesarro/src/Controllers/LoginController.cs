using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectodesarro.Models;
using proyectodesarro.Data;

namespace proyectodesarro.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string nombre, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre == nombre && u.Contrasena == contrasena);

            if (usuario != null)
            {
                // En una aplicación real, aquí deberías usar una autenticación adecuada
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            return View();
        }

        public IActionResult CerrarSesion()
        {
            // Aquí podrías agregar cualquier limpieza de sesión necesaria
            return RedirectToAction("Index", "Login");
        }
    }
}
