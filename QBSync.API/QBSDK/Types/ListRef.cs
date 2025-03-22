using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

public class ListRef
{
    public string? ListID { get; set; }
    public string? FullName { get; set; }

    public XElement ToXElement(string name)
    {
        var result = new XElement(name);
        result.Append(ListID);
        result.Append(FullName);
        return result;
    }
}

internal static class ListRefExtensions
{
    public static void Append(this XElement element, ListRef? listRef, [CallerArgumentExpression(nameof(listRef))] string name = "")
    {
        if (listRef != null)
        {
            element.Add(listRef.ToXElement(name));
        }
    }
}
