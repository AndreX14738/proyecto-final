using proyectodesarro.Models;

namespace proyectodesarro.Helpers
{    public static class UsuarioHelper
    {
        private static string filePath = "usuarios.csv";

        public static bool ValidarUsuario(string nombre, string contrasena)
        {
            if (!File.Exists(filePath)) return false;

            var lineas = File.ReadAllLines(filePath);            foreach (var linea in lineas)
            {
                var partes = linea.Split(',');
                if (partes.Length >= 2)
                {
                    if (partes[0] == nombre && partes[1] == contrasena)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
