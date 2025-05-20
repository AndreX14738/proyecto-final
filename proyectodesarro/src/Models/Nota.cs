using System;

namespace proyectodesarro.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public required int EstudianteId { get; set; }
        public required string Materia { get; set; }
        public required string Periodo { get; set; } // Ejemplo: "2024-1", "2024-2"
        public decimal Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Observaciones { get; set; }

        public Nota()
        {
            FechaRegistro = DateTime.Now;
            Materia = string.Empty;
            Periodo = string.Empty;
        }
    }
}
