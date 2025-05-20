namespace ProyectoDesarro.Models 
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }
        
        // Constructor vacío
        public Usuario() 
        {
        }
        
        // Constructor con parámetros
        public Usuario(string nombre, string contrasena) 
        {
            Nombre = nombre;
            Contrasena = contrasena;
        }
        
        // Constructor completo
        public Usuario(int id, string nombre, string contrasena, string email)
        {
            Id = id;
            Nombre = nombre;
            Contrasena = contrasena;
            Email = email;
        }
        
        // Método para crear un usuario predeterminado fácil
        public static Usuario CrearUsuarioPredeterminado()
        {
            return new Usuario
            {
                Id = 1,
                Nombre = "admin",
                Contrasena = "123456",
                Email = "admin@ejemplo.com"
            };
        }
        
        // Método para mostrar información (sin mostrar la contraseña por seguridad)
        public string ObtenerInformacion()
        {
            return $"Usuario ID: {Id}, Nombre: {Nombre}, Email: {Email}";
        }
    }
}