using System;
using System.Collections.Generic;

public struct DefaultValue
{
    public Type TypeInfo;
    public string Value;
}

public interface IPropertyable
{
    /// <summary>
    /// Returns the set of properties recognized by the object, their types, and default values.
    /// </summary>
    Dictionary<string, DefaultValue> DefaultProperties
    {
        get;
    }

    /// <summary>
    /// When queried, this should be a set of all properties whose values differ from their
    /// default values.
    /// 
    /// When set, objects should set any non-included properties to their default values.
    /// </summary>
    /// <exception cref="PropertyException">
    /// Implementations may choose to throw a PropertyException if an invalid value for a
    /// property is given, or an unknown property is passed. Note that some implementations
    /// may choose to interpret invalid values as default values and may simply ignore or
    /// even store or create unknown properties.
    /// </exception>
    Dictionary<string, string> Properties
    {
        get;
        set;
    }
}