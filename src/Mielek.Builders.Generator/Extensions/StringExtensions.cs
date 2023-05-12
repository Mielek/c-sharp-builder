namespace Mielek.Builders.Generator.Extensions;

public static class StringExtensions
{
    public static string ToMethodName(this string variableName)
    {
        var methodName = variableName.Replace("_", "");
        methodName = $"{methodName[0].ToString().ToUpper()}{methodName.Remove(0, 1)}";
        return methodName;
    }
}