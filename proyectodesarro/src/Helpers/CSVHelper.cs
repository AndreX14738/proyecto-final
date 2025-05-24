using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using proyectodesarro.Models;

namespace proyectodesarro.Helpers
{    public static class CSVHelper
    {
        private static string estudiantesPath = "estudiantes.csv";
        private static string notasPath = "notas.csv";
        private static string asistenciasPath = "asistencias.csv";
        private static string usuariosPath = "usuarios.csv";

        public static void GuardarEstudiante(Estudiante estudiante)
        {
            var estudiantes = LeerEstudiantes();
            if (estudiante.Id == 0)
            {
                estudiante.Id = estudiantes.Count > 0 ? estudiantes.Max(e => e.Id) + 1 : 1;
            }
            
            string linea = $"{estudiante.Id},{estudiante.Nombre},{estudiante.Apellidos},{estudiante.Correo}," +
                         $"{estudiante.FechaNacimiento:yyyy-MM-dd},{estudiante.Direccion},{estudiante.Telefono}," +
                         $"{estudiante.DocumentoIdentidad},{estudiante.FechaIngreso:yyyy-MM-dd},{estudiante.Estado}," +
                         $"{estudiante.Grado},{estudiante.Seccion},{estudiante.DocenteId},{estudiante.Materia}," +
                         $"{estudiante.AsistenciaInicial},{estudiante.NotaInicial}";
            
            File.AppendAllText(estudiantesPath, linea + Environment.NewLine);
        }

        public static List<Estudiante> LeerEstudiantes()
        {
            if (!File.Exists(estudiantesPath))
            {
                return new List<Estudiante>();
            }

            var estudiantes = new List<Estudiante>();
            var lineas = File.ReadAllLines(estudiantesPath);

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 12) // Permitimos leer archivos antiguos que no tienen los nuevos campos
                {
                    estudiantes.Add(new Estudiante
                    {
                        Id = int.Parse(datos[0]),
                        Nombre = datos[1],
                        Apellidos = datos[2],
                        Correo = datos[3],
                        FechaNacimiento = DateTime.Parse(datos[4]),
                        Direccion = datos[5],
                        Telefono = datos[6],
                        DocumentoIdentidad = datos[7],
                        FechaIngreso = DateTime.Parse(datos[8]),
                        Estado = datos[9],
                        Grado = datos[10],
                        Seccion = datos[11],
                        DocenteId = datos.Length > 12 ? int.Parse(datos[12]) : 1,
                        Materia = datos.Length > 13 ? datos[13] : "Matemáticas",
                        AsistenciaInicial = datos.Length > 14 ? datos[14] : "Presente",
                        NotaInicial = datos.Length > 15 && !string.IsNullOrEmpty(datos[15]) ? decimal.Parse(datos[15]) : null
                    });
                }
            }

            return estudiantes;
        }

        public static void GuardarNota(Nota nota)
        {
            var notas = LeerNotas(nota.EstudianteId);
            if (nota.Id == 0)
            {
                nota.Id = notas.Count > 0 ? notas.Max(n => n.Id) + 1 : 1;
            }
            
            string linea = $"{nota.Id},{nota.EstudianteId},{nota.Materia},{nota.Periodo}," +
                         $"{nota.Valor},{nota.FechaRegistro:yyyy-MM-dd HH:mm:ss},{nota.Observaciones}";
            
            File.AppendAllText(notasPath, linea + Environment.NewLine);
        }

        public static List<Nota> LeerNotas(int estudianteId)
        {
            if (!File.Exists(notasPath))
            {
                return new List<Nota>();
            }

            var notas = new List<Nota>();
            var lineas = File.ReadAllLines(notasPath);

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 7 && int.Parse(datos[1]) == estudianteId)
                {
                    notas.Add(new Nota
                    {
                        Id = int.Parse(datos[0]),
                        EstudianteId = estudianteId,
                        Materia = datos[2],
                        Periodo = datos[3],
                        Valor = decimal.Parse(datos[4]),
                        FechaRegistro = DateTime.Parse(datos[5]),
                        Observaciones = datos[6]
                    });
                }
            }

            return notas;
        }        public static List<Nota> LeerTodasLasNotas()
        {
            if (!File.Exists(notasPath))
            {
                return new List<Nota>();
            }

            var notas = new List<Nota>();
            var lineas = File.ReadAllLines(notasPath);
            var nextId = 1;

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 7)
                {
                    notas.Add(new Nota
                    {
                        Id = nextId++,
                        EstudianteId = int.Parse(datos[1]),
                        Materia = datos[2],
                        Periodo = datos[3],
                        Valor = decimal.Parse(datos[4]),
                        FechaRegistro = DateTime.Parse(datos[5]),
                        Observaciones = datos[6]
                    });
                }
            }

            return notas;
        }

        public static void GuardarAsistencia(Asistencia asistencia)
        {
            var asistencias = LeerAsistencias(asistencia.EstudianteId);
            if (asistencia.Id == 0)
            {
                asistencia.Id = asistencias.Count > 0 ? asistencias.Max(a => a.Id) + 1 : 1;
            }
            
            string linea = $"{asistencia.Id},{asistencia.EstudianteId},{asistencia.Materia}," +
                         $"{asistencia.Fecha:yyyy-MM-dd HH:mm:ss},{asistencia.Estado},{asistencia.Observaciones}";
            
            File.AppendAllText(asistenciasPath, linea + Environment.NewLine);
        }

        public static List<Asistencia> LeerAsistencias(int estudianteId)
        {
            if (!File.Exists(asistenciasPath))
            {
                return new List<Asistencia>();
            }

            var asistencias = new List<Asistencia>();
            var lineas = File.ReadAllLines(asistenciasPath);

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 6 && int.Parse(datos[1]) == estudianteId)
                {
                    asistencias.Add(new Asistencia
                    {
                        Id = int.Parse(datos[0]),
                        EstudianteId = estudianteId,
                        Materia = datos[2],
                        Fecha = DateTime.Parse(datos[3]),
                        Estado = datos[4],
                        Observaciones = datos[5]
                    });
                }
            }

            return asistencias;
        }

        public static List<Asistencia> LeerTodasLasAsistencias()
        {
            if (!File.Exists(asistenciasPath))
            {
                return new List<Asistencia>();
            }

            var asistencias = new List<Asistencia>();
            var lineas = File.ReadAllLines(asistenciasPath);

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 6)
                {
                    asistencias.Add(new Asistencia
                    {
                        Id = int.Parse(datos[0]),
                        EstudianteId = int.Parse(datos[1]),
                        Materia = datos[2],
                        Fecha = DateTime.Parse(datos[3]),
                        Estado = datos[4],
                        Observaciones = datos[5]
                    });
                }
            }

            return asistencias;
        }

        public static List<Usuario> LeerUsuarios()
        {
            if (!File.Exists(usuariosPath))
            {
                return new List<Usuario> { Usuario.CrearUsuarioPredeterminado() };
            }

            var usuarios = new List<Usuario>();
            var lineas = File.ReadAllLines(usuariosPath);
            int id = 1;

            foreach (var linea in lineas)
            {
                var datos = linea.Split(',');
                if (datos.Length >= 2)
                {
                    usuarios.Add(new Usuario
                    {
                        Id = id++,
                        Nombre = datos[0],
                        Email = datos.Length > 2 ? datos[2] : $"{datos[0]}@ejemplo.com",
                        Contrasena = datos[1],
                        FechaRegistro = DateTime.Now
                    });
                }
            }

            return usuarios;
        }
    }
}

