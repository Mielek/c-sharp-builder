namespace Mielek.Builders.Generator.Field;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Builders.Generator.Extensions;

public class SimpleFieldHandlerProvider : IFieldSetterHandlerProvider
{
    public bool TryGetHandler(FieldDeclarationSyntax field, out IFieldSetterHandler handler)
    {
        handler = new SimpleFieldHandler(field);
        return true;
    }

    public class SimpleFieldHandler : IFieldSetterHandler
    {

        readonly FieldDeclarationSyntax _field;

        public SimpleFieldHandler(FieldDeclarationSyntax field)
        {
            _field = field;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToMethodName();
                var paramType = _field.Declaration.Type.ToString().Replace("?", "");
                builder.Field(new BuilderField(paramType, variableName));
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"{paramType} value" },
                    new[] { $"{variableName.ToFieldName()} = value;" }
                ));
            }
        }

    }
}