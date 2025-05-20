using System;

namespace proyectodesarro.Models
{
    public class Asistencia
    {
        public int Id { get; set; }
        public required int EstudianteId { get; set; }
        public required string Materia { get; set; }
        public DateTime Fecha { get; set; }
        public required string Estado { get; set; } // Presente, Ausente, Tardanza, Justificado
        public string? Observaciones { get; set; }

        public Asistencia()
        {
            Fecha = DateTime.Now;
            Estado = "Presente";
            Materia = string.Empty;
        }
    }
}
