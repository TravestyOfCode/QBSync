using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

internal static class XElementExtensions
{
    public static void Append(this XElement element, string? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        if (value != null)
        {
            element.Add(new XElement(name, value));
        }
    }
    public static void Append(this XElement element, DateOnly? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        if (value != null)
        {
            element.Add(new XElement(name, value.Value.ToString("yyyy-DD-mm")));
        }
    }
    public static void Append(this XElement element, decimal? value, int decimalPlaces, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        if (value != null)
        {
            element.Add(new XElement(name, value.Value.ToString($"F{decimalPlaces}")));
        }
    }

}
