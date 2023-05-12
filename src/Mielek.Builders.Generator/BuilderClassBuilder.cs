namespace Mielek.Builders.Generator;

using System.Collections.Generic;
using System.Text;

public class BuilderClassBuilder
{
    readonly string _namespaceName;
    readonly string _className;
    readonly string _buildedClassName;

    readonly StringBuilder _sourceBuilder = new();

    readonly List<string> _usingsTypes = new();
    readonly List<BuilderField> _fields = new();
    readonly List<BuilderSetMethod> _methods = new();

    public BuilderClassBuilder(string namespaceName, string className)
    {
        _namespaceName = namespaceName;
        _buildedClassName = className;
        _className = $"{className}Builder";
    }

    public BuilderClassBuilder Using(string usingType)
    {
        _usingsTypes.Add(usingType);
        return this;
    }

    public BuilderClassBuilder Field(BuilderField field)
    {
        _fields.Add(field);
        return this;
    }

    public BuilderClassBuilder Method(BuilderSetMethod method)
    {
        _methods.Add(method);
        return this;
    }

    public string Build()
    {
        AppendUsings();
        AppendNamespace();
        AppendClassStart();
        AppendFields();
        AppendMethods();
        AppendBuild();
        AppendClassEnd();
        return _sourceBuilder.ToString();
    }

    private void AppendUsings()
    {
        foreach (var usingType in _usingsTypes)
        {
            _sourceBuilder.Append("using ");
            _sourceBuilder.Append(usingType);
            _sourceBuilder.Append(";\n");
        }
        _sourceBuilder.Append('\n');
    }

    private void AppendNamespace()
    {
        _sourceBuilder.Append("namespace ");
        _sourceBuilder.Append(_namespaceName);
        _sourceBuilder.Append(";\n\n");
    }


    private void AppendClassStart()
    {
        _sourceBuilder.Append("public partial class ");
        _sourceBuilder.Append(_className);
        _sourceBuilder.Append("\n{\n");
    }

    private void AppendFields()
    {
        foreach (var field in _fields)
        {
            AppendIntend(1);
            _sourceBuilder.Append("private ");
            _sourceBuilder.Append(field.Type);
            _sourceBuilder.Append("? ");
            _sourceBuilder.Append(field.Name);
            _sourceBuilder.Append(";\n");
        }
    }

    private void AppendMethods()
    {
        foreach (var method in _methods)
        {
            AppendIntend(1);
            _sourceBuilder.Append("public ");
            _sourceBuilder.Append(_className);
            _sourceBuilder.Append(' ');
            _sourceBuilder.Append(method.Name);
            _sourceBuilder.Append('(');

            if (method.Params.Length > 0)
            {
                foreach (var param in method.Params)
                {
                    _sourceBuilder.Append(param);
                    _sourceBuilder.Append(", ");
                }
                _sourceBuilder.Remove(_sourceBuilder.Length - 2, 2); // remove separator
            }

            _sourceBuilder.Append(")\n");
            AppendLineOfCode(1, "{");

            foreach (var line in method.BodyLines)
            {
                AppendLineOfCode(2, line);
            }
            AppendLineOfCode(2, "return this;");
            AppendLineOfCode(1, "}\n");
        }
    }

    private void AppendBuild()
    {
        AppendIntend(1);
        _sourceBuilder.Append("public ");
        _sourceBuilder.Append(_buildedClassName);
        _sourceBuilder.Append(" Build()\n");
        AppendLineOfCode(1, "{");
        AppendIntend(2);
         _sourceBuilder.Append("return new ");
        _sourceBuilder.Append(_buildedClassName);
        _sourceBuilder.Append("()\n");
        AppendLineOfCode(2, "{");
        
        _sourceBuilder.Append(string.Join(",\n", _fields.Select(field => $"            {field.Name} = this.{field.Name}")));
        _sourceBuilder.Append('\n');
        AppendLineOfCode(2, "};");
        AppendLineOfCode(1, "}");
    }

    private void AppendClassEnd()
    {
        _sourceBuilder.Append("\n}");
    }


    private void AppendIntend(int intend)
    {
        for (int i = 0; i < intend; i++)
        {
            _sourceBuilder.Append("    ");
        }
    }

    private void AppendLineOfCode(int intend, string line)
    {
        AppendIntend(intend);
        _sourceBuilder.Append(line);
        _sourceBuilder.Append('\n');
    }
}

public record BuilderSetMethod(string Name, string[] Params, string[] BodyLines);
public record BuilderField(string Type, string Name);