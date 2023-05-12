namespace Mielek.Builders.Generator;

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Builders.Generator.Attributes;
using Mielek.Builders.Generator.Extensions;
using Mielek.Builders.Generator.Field;

class ClassBuilder
{
    private static readonly IFieldSetterHandlerProvider[] MethodProviders = new IFieldSetterHandlerProvider[]
    {
        new SimpleFieldHandlerProvider()
    };

    readonly ClassDeclarationSyntax _classDeclaration;
    readonly BuilderClassBuilder _classBuilder;

    public ClassBuilder(ClassDeclarationSyntax classDeclaration)
    {
        _classDeclaration = classDeclaration;
        var namespaceName = classDeclaration.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().First().Name.ToString();
        _classBuilder = new BuilderClassBuilder(namespaceName, classDeclaration.Identifier.Text);
    }

    public string Build()
    {
        AddFields();
        return _classBuilder.Build();
    }

    private void AddFields()
    {
        var fields = _classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
        foreach (var field in fields)
        {
            if (field.IsPublic() && !field.HasAttribute(nameof(IgnoreFieldAttribute)))
            {
                HandleField(field);
            }
        }
    }

    private void HandleField(FieldDeclarationSyntax field)
    {
        foreach (var provider in MethodProviders)
        {
            if(provider.TryGetHandler(field, out var handler))
            {
                handler.Handle(_classBuilder);
                return;
            }
        }
        throw new Exception();
    }
}