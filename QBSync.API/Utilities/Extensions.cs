namespace QBSync.API;

public static class Extensions
{
    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrWhiteSpace(value);
    public static bool IsNotNullOrEmpty(this string? value) => !string.IsNullOrWhiteSpace(value);
}
