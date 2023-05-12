namespace Mielek.Builders.Generator.Field;

using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface IFieldSetterHandlerProvider
{
    bool TryGetHandler(FieldDeclarationSyntax field, out IFieldSetterHandler handler);
}