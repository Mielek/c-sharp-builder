namespace Mielek.Builders.Generator.Extensions;

public static class StringExtensions
{
    public static string ToMethodName(this string variableName) =>
        $"{variableName[0].ToString().ToUpper()}{variableName.Remove(0, 1)}";

    public static string ToFieldName(this string variableName) =>
        $"{variableName[0].ToString().ToLower()}{variableName.Remove(0, 1)}";
}