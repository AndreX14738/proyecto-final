using System.Collections.Generic;

namespace ProyectoDesarro.Models 
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        
        public Estudiante() { }
        
        public Estudiante(int id, string nombre, string correo)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
        }
        
        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Correo: {Correo}";
        }
        
        // Lista de ejemplo con 10 estudiantes precargados
        public static List<Estudiante> Estudiantes = new List<Estudiante>
        {
            new Estudiante(1, "Juan Pérez", "juan.perez@mail.com"),
            new Estudiante(2, "Ana Gómez", "ana.gomez@mail.com"),
            new Estudiante(3, "Luis Martínez", "luis.martinez@mail.com"),
            new Estudiante(4, "María Rodríguez", "maria.rodriguez@mail.com"),
            new Estudiante(5, "Carlos López", "carlos.lopez@mail.com"),
            new Estudiante(6, "Sofía Torres", "sofia.torres@mail.com"),
            new Estudiante(7, "Pablo Ramírez", "pablo.ramirez@mail.com"),
            new Estudiante(8, "Lucía Fernández", "lucia.fernandez@mail.com"),
            new Estudiante(9, "Roberto Sánchez", "roberto.sanchez@mail.com"),
            new Estudiante(10, "Valentina Díaz", "valentina.diaz@mail.com")
        };
        
        // Método para obtener todos los estudiantes
        public static List<Estudiante> ObtenerTodosLosEstudiantes()
        {
            return Estudiantes;
        }
        
        // Método para buscar un estudiante por ID
        public static Estudiante BuscarPorId(int id)
        {
            return Estudiantes.Find(e => e.Id == id);
        }
    }
}
