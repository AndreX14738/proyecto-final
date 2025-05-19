using System.Collections.Generic;

namespace proyectodesarro.Models
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

        // Lista de ejemplo con estudiantes precargados
        public static List<Estudiante> EstudiantesEjemplo = new List<Estudiante>
        {
            new Estudiante(1, "Juan Perez", "juan.perez@mail.com"),
            new Estudiante(2, "Ana Gómez", "ana.gomez@mail.com"),
            new Estudiante(3, "Luis Martínez", "luis.martinez@mail.com")
        };
    }
}
