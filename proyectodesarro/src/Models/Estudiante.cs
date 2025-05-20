using System;
using System.Collections.Generic;

namespace proyectodesarro.Models 
{
    public class Estudiante
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public required string DocumentoIdentidad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public required string Estado { get; set; } // Activo, Inactivo, Graduado, etc.
        public required string Grado { get; set; }
        public required string Seccion { get; set; }
        
        public Estudiante() 
        {
            FechaIngreso = DateTime.Now;
            Estado = "Activo";
            Nombre = string.Empty;
            Apellidos = string.Empty;
            Correo = string.Empty;
            DocumentoIdentidad = string.Empty;
            Grado = string.Empty;
            Seccion = string.Empty;
        }
        
        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre} {Apellidos}, Correo: {Correo}";
        }
        
        // Lista de ejemplo con estudiantes precargados
        public static List<Estudiante> Estudiantes = new List<Estudiante>
        {
            new Estudiante 
            { 
                Id = 1, 
                Nombre = "Juan", 
                Apellidos = "Pérez", 
                Correo = "juan.perez@mail.com",
                DocumentoIdentidad = "12345678",
                Grado = "1",
                Seccion = "A",
                Estado = "Activo"
            },
            new Estudiante 
            { 
                Id = 2, 
                Nombre = "Ana", 
                Apellidos = "Gómez", 
                Correo = "ana.gomez@mail.com",
                DocumentoIdentidad = "23456789",
                Grado = "1",
                Seccion = "A",
                Estado = "Activo"
            },
            // ... más estudiantes de ejemplo
        };
        
        // Método para obtener todos los estudiantes
        public static List<Estudiante> ObtenerTodosLosEstudiantes()
        {
            return Estudiantes;
        }
        
        // Método para buscar un estudiante por ID
        public static Estudiante? BuscarPorId(int id)
        {
            return Estudiantes.Find(e => e.Id == id);
        }
    }
}
