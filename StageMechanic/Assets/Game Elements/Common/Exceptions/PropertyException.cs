using System;

public class PropertyException : Exception
{
    public PropertyException()
    {
    }

    public PropertyException(string message)
        : base(message)
    {
    }

    public PropertyException(string message, Exception inner)
        : base(message, inner)
    {
    }
}