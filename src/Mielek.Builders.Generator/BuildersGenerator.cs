namespace Mielek.Builders.Generator;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Mielek.Builders.Generator.Attributes;
using Mielek.Builders.Generator.Extensions;

[Generator]
public class BuildersGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var classBuilders = GenerateBuilder(context.Compilation, syntaxTree);
            foreach (var classBuilder in classBuilders)
            {
                context.AddSource($"{classBuilder.Key}.Builder.cs", SourceText.From(classBuilder.Value, Encoding.UTF8));
            }
        }
    }

    private Dictionary<string, string> GenerateBuilder(Compilation compilation, SyntaxTree syntaxTree)
    {
        var classToBuilder = new Dictionary<string, string>();

        var root = syntaxTree.GetRoot();
        var classesWithAttribute = root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(cds => cds.HasAttribute(nameof(GenerateBuilderAttribute)))
            .ToList();

        foreach (var classDeclaration in classesWithAttribute)
        {
            var className = classDeclaration.Identifier.Text;
            classToBuilder[className] = new ClassBuilder(classDeclaration).Build();
        }

        return classToBuilder;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}