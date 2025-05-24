using System.Globalization;

namespace app.Utils
{
    public static class Formatter
    {
        public static string FormatPrice(decimal amount)
        {
            return amount.ToString("C0", new CultureInfo("es-CO"));
        }
    }
}