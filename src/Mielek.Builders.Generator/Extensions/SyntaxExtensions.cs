namespace Mielek.Builders.Generator.Extensions;

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


public static class SyntaxExtensions
{
    public static bool HasAttribute(this MemberDeclarationSyntax member, string name)
    {
        string fullname, shortname;
        var attrLen = "Attribute".Length;
        if (name.EndsWith("Attribute"))
        {
            fullname = name;
            shortname = name.Remove(name.Length - attrLen, attrLen);
        }
        else
        {
            fullname = name + "Attribute";
            shortname = name;
        }

        return member.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString().StartsWith(shortname)));
    }

    public static T FindParent<T>(this SyntaxNode node) where T : class
    {
        var current = node;
        while (true)
        {
            current = current.Parent;
            if (current == null) throw new NullReferenceException($"Not found parent {nameof(T)}");
            if (current is T casted) return casted;
        }
    }

    public static bool IsPublic(this MemberDeclarationSyntax member) =>
        member.Modifiers.Any(token => token.IsKind(SyntaxKind.PublicKeyword));
}