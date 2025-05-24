using Microsoft.EntityFrameworkCore;
using proyectodesarro.Data;
using proyectodesarro.Helpers;
using proyectodesarro.Models;

namespace proyectodesarro.Services
{
    public class MigracionService
    {
        private readonly ApplicationDbContext _context;

        public MigracionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task MigrarDatosAsync()
        {
            try
            {
                // Migrar usuarios
                var usuarios = CSVHelper.LeerUsuarios();
                var usuariosExistentes = await _context.Usuarios.Select(u => u.Id).ToListAsync();
                foreach (var usuario in usuarios.Where(u => !usuariosExistentes.Contains(u.Id)))
                {
                    _context.Usuarios.Add(usuario);
                }
                await _context.SaveChangesAsync();

                // Migrar estudiantes
                var estudiantes = CSVHelper.LeerEstudiantes();
                var estudiantesExistentes = await _context.Estudiantes.Select(e => e.Id).ToListAsync();
                foreach (var estudiante in estudiantes.Where(e => !estudiantesExistentes.Contains(e.Id)))
                {
                    _context.Estudiantes.Add(estudiante);
                }
                await _context.SaveChangesAsync();

                // Migrar notas
                var notas = CSVHelper.LeerTodasLasNotas();
                var notasExistentes = await _context.Notas.Select(n => n.Id).ToListAsync();
                foreach (var nota in notas.Where(n => !notasExistentes.Contains(n.Id)))
                {
                    _context.Notas.Add(nota);
                }
                await _context.SaveChangesAsync();

                // Migrar asistencias
                var asistencias = CSVHelper.LeerTodasLasAsistencias();
                var asistenciasExistentes = await _context.Asistencias.Select(a => a.Id).ToListAsync();
                foreach (var asistencia in asistencias.Where(a => !asistenciasExistentes.Contains(a.Id)))
                {
                    _context.Asistencias.Add(asistencia);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la migración: {ex.Message}");
                throw;
            }
        }
    }
}
