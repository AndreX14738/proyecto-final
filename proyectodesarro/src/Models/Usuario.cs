namespace ProyectoDesarro.Models 
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Contrasena { get; set; }
        public required string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        
        // Constructor vacío
        public Usuario() 
        {
            FechaRegistro = DateTime.Now;
            Nombre = string.Empty;
            Contrasena = string.Empty;
            Email = string.Empty;
        }
        
        // Constructor con parámetros
        public Usuario(string nombre, string contrasena) 
        {
            Nombre = nombre;
            Contrasena = contrasena;
            FechaRegistro = DateTime.Now;
            Email = string.Empty;
        }
        
        // Constructor completo
        public Usuario(int id, string nombre, string contrasena, string email)
        {
            Id = id;
            Nombre = nombre;
            Contrasena = contrasena;
            Email = email;
            FechaRegistro = DateTime.Now;
        }
        
        // Método para crear un usuario predeterminado fácil
        public static Usuario CrearUsuarioPredeterminado()
        {
            return new Usuario
            {
                Id = 1,
                Nombre = "admin",
                Contrasena = "123456",
                Email = "admin@ejemplo.com",
                FechaRegistro = DateTime.Now
            };
        }
        
        // Método para mostrar información (sin mostrar la contraseña por seguridad)
        public string ObtenerInformacion()
        {
            return $"Usuario ID: {Id}, Nombre: {Nombre}, Email: {Email}, Fecha de Registro: {FechaRegistro}";
        }
    }
}