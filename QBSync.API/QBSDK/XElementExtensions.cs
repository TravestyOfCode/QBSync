using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

internal static class XElementExtensions
{
    public static XDocument CreateQBXML(this XElement request, OnError onError = OnError.stopOnError)
    {
        var msgsRq = new XElement("QBXMLMsgsRq", request);
        msgsRq.Add(new XAttribute(nameof(onError), onError));

        var result = new XDocument(new XProcessingInstruction("qbxml", "version=\"13.0\""));
        result.Declaration = new XDeclaration("1.0", "utf-8", null);

        result.Add(new XElement("QBXML", msgsRq));
        return result;
    }

    public static string ToXML(this XDocument document)
    {
        var builder = new StringBuilder();
        using (TextWriter writer = new StringWriter(builder))
        {
            document.Save(writer);
            return builder.ToString();
        }
    }

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
            element.Add(new XElement(name, value.Value.ToString("yyyy-MM-dd")));
        }
    }
    public static void Append(this XElement element, decimal? value, int decimalPlaces = 2, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        if (value != null)
        {
            element.Add(new XElement(name, value.Value.ToString($"F{decimalPlaces}")));
        }
    }

    public static string? AsString(this XElement? element) => element?.Value;
    public static decimal AsDecimalOrValue(this XElement? element, decimal defaultValue = 0.0m) => element == null ? defaultValue : decimal.TryParse(element.Value, out var value) ? value : defaultValue;
    public static decimal AsDecimal(this XElement element) => decimal.TryParse(element?.Value, out var value) ? value : throw new InvalidOperationException("Unable to convert XElement to decimal.");

}
