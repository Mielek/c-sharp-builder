namespace Mielek.Builders.Generator.Attributes;

using System;

[AttributeUsage(AttributeTargets.Field)]
public class IgnoreFieldAttribute : Attribute
{
    public IgnoreFieldAttribute()
    { }
}