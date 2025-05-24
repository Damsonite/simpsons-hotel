using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpsonsHotel.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CargaArchivoAttribute : Attribute
    {
        public string RutaArchivo { get; set; }
    }

    public class CargaArchivoAspect
    {
        public async Task<string[]> CargarArchivo(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivo}");
                }

                return await File.ReadAllLinesAsync(rutaArchivo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar el archivo {rutaArchivo}: {ex.Message}", ex);
            }
        }

        public async Task GuardarArchivo(string rutaArchivo, string[] lineas)
        {
            try
            {
                var directorio = Path.GetDirectoryName(rutaArchivo);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                await File.WriteAllLinesAsync(rutaArchivo, lineas);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo {rutaArchivo}: {ex.Message}", ex);
            }
        }
    }
} 