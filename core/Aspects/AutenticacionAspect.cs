using System.Reflection;

namespace SimpsonsHotel.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiereAutenticacionAttribute : Attribute
    {
        public string RolRequerido { get; set; }
    }

    public class AutenticacionAspect
    {
        private static string _usuarioActual;
        private static string _rolActual;

        public static void Autenticar(string usuario, string rol)
        {
            _usuarioActual = usuario;
            _rolActual = rol;
        }

        public static void CerrarSesion()
        {
            _usuarioActual = null;
            _rolActual = null;
        }

        public static bool EstaAutenticado()
        {
            return !string.IsNullOrEmpty(_usuarioActual);
        }

        public static bool TieneRol(string rol)
        {
            return _rolActual == rol;
        }

        public static async Task<T> ValidarAutenticacion<T>(object target, MethodInfo method, object[] parameters)
        {
            var authAttr = method.GetCustomAttributes(typeof(RequiereAutenticacionAttribute), true)
                .FirstOrDefault() as RequiereAutenticacionAttribute;
                
            if (authAttr == null)
            {
                return (T)method.Invoke(target, parameters);
            }

            if (!EstaAutenticado())
            {
                throw new UnauthorizedAccessException("Se requiere autenticación para acceder a este recurso");
            }

            if (!string.IsNullOrEmpty(authAttr.RolRequerido) && !TieneRol(authAttr.RolRequerido))
            {
                throw new UnauthorizedAccessException($"Se requiere el rol {authAttr.RolRequerido} para acceder a este recurso");
            }

            var result = method.Invoke(target, parameters);
            if (result is Task task)
            {
                await task;
                result = task.GetType().GetProperty("Result")?.GetValue(task);
            }

            return (T)result;
        }
    }
} 