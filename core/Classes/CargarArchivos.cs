using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpsonsHotel.Core.Classes
{
    public class CargarArchivos
    {
        public static List<Cliente> CargarClientes(string rutaArchivo)
        {
            var clientes = new List<Cliente>();
            
            try
            {
                if (!File.Exists(rutaArchivo))
                    return clientes;
                
                var lineas = File.ReadAllLines(rutaArchivo);
                
                foreach (var linea in lineas.Skip(1)) // Saltamos la primera línea (encabezado)
                {
                    var datos = linea.Split(',');
                    if (datos.Length >= 6)
                    {
                        try
                        {
                            var nombre = datos[0].Trim();
                            var tipoDocumento = ParseTipoDocumento(datos[1].Trim());
                            var numeroDocumento = datos[2].Trim();
                            var telefono = datos[3].Trim();
                            var codigoFidelidad = datos[5].Trim();
                            
                            var cliente = new Cliente(nombre, tipoDocumento, numeroDocumento, telefono, codigoFidelidad);
                            clientes.Add(cliente);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al procesar línea de cliente: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar archivo de clientes: {ex.Message}");
            }
            
            return clientes;
        }
        
        public static List<Huesped> CargarHuespedes(string rutaArchivo)
        {
            var huespedes = new List<Huesped>();
            
            try
            {
                if (!File.Exists(rutaArchivo))
                    return huespedes;
                
                var lineas = File.ReadAllLines(rutaArchivo);
                
                foreach (var linea in lineas.Skip(1)) // Saltamos la primera línea (encabezado)
                {
                    var datos = linea.Split(',');
                    if (datos.Length >= 5)
                    {
                        try
                        {
                            var nombre = datos[0].Trim();
                            var tipoDocumento = ParseTipoDocumento(datos[1].Trim());
                            var numeroDocumento = datos[2].Trim();
                            var telefono = datos[3].Trim();
                            
                            var huesped = new Huesped(nombre, tipoDocumento, numeroDocumento, telefono);
                            huespedes.Add(huesped);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al procesar línea de huésped: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar archivo de huéspedes: {ex.Message}");
            }
            
            return huespedes;
        }
        
        private static Persona.TipoId ParseTipoDocumento(string tipo)
        {
            return tipo.ToUpper() switch
            {
                "CC" => Persona.TipoId.CC,
                "TI" => Persona.TipoId.TI,
                "CE" => Persona.TipoId.CE,
                "PA" => Persona.TipoId.PA,
                _ => Persona.TipoId.CC // Por defecto
            };
        }
    }
}
