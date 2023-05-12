namespace Mielek.Builders.Generator.Attributes;

using System;


[AttributeUsage(AttributeTargets.Class)]
public class GenerateBuilderAttribute : Attribute
{
    public GenerateBuilderAttribute()
    { }
}