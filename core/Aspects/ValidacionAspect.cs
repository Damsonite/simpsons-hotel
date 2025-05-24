using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpsonsHotel.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidacionAttribute : Attribute
    {
        public string MensajeError { get; set; }
    }

    public class ValidacionAspect
    {
        private readonly ILogger<ValidacionAspect> _logger;

        public ValidacionAspect(ILogger<ValidacionAspect> logger)
        {
            _logger = logger;
        }

        public async Task<T> Validar<T>(object target, MethodInfo method, object[] parameters)
        {
            try
            {
                // Validar que el método tenga el atributo de validación
                var validacionAttr = method.GetCustomAttribute<ValidacionAttribute>();
                if (validacionAttr == null)
                {
                    return (T)method.Invoke(target, parameters);
                }

                // Validar los parámetros
                foreach (var param in parameters)
                {
                    if (param == null)
                    {
                        throw new ArgumentNullException($"El parámetro no puede ser nulo: {validacionAttr.MensajeError}");
                    }

                    // Validar strings vacíos
                    if (param is string str && string.IsNullOrWhiteSpace(str))
                    {
                        throw new ArgumentException($"El parámetro no puede estar vacío: {validacionAttr.MensajeError}");
                    }

                    // Validar números negativos
                    if (param is int num && num < 0)
                    {
                        throw new ArgumentException($"El número no puede ser negativo: {validacionAttr.MensajeError}");
                    }

                    if (param is decimal dec && dec < 0)
                    {
                        throw new ArgumentException($"El número no puede ser negativo: {validacionAttr.MensajeError}");
                    }
                }

                // Ejecutar el método
                var result = method.Invoke(target, parameters);
                
                // Si el método es asíncrono, esperar el resultado
                if (result is Task task)
                {
                    await task;
                    result = task.GetType().GetProperty("Result")?.GetValue(task);
                }

                return (T)result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la validación: {Message}", ex.Message);
                throw;
            }
        }
    }
} 