using proyectodesarro.Models;
using System.Collections.Generic;
using System.IO;

namespace proyectodesarro.Helpers
{
    public static class CSVHelper
    {
        private static string filePath = "estudiantes.csv";

        public static List<Estudiante> LeerEstudiantes()
        {
            var lista = new List<Estudiante>();
            if (!File.Exists(filePath)) return lista;

            foreach (var linea in File.ReadAllLines(filePath))
            {
                var partes = linea.Split(',');
                if (partes.Length == 3)
                {
                    lista.Add(new Estudiante
                    {
                        Id = int.Parse(partes[0]),
                        Nombre = partes[1],
                        Correo = partes[2]
                    });
                }
            }

            return lista;
        }

        public static void GuardarEstudiante(Estudiante estudiante)
        {
            int nuevoId = LeerEstudiantes().Count + 1;
            estudiante.Id = nuevoId;
            var linea = $"{estudiante.Id},{estudiante.Nombre},{estudiante.Correo}";
            File.AppendAllText(filePath, linea + "\n");
        }
    }
}
