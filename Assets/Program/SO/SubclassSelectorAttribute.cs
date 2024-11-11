using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SubclassSelectorAttribute : PropertyAttribute
{
    private readonly bool _includeMono;

    public SubclassSelectorAttribute(bool includeMono = false)
    {
        _includeMono = includeMono;
    }

    public bool IsIncludeMono()
    {
        return _includeMono;
    }
}