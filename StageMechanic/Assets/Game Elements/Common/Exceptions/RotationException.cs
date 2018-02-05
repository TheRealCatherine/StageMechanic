using System;

public class RotationException : Exception
{
    public RotationException()
    {
    }

    public RotationException(string message)
        : base(message)
    {
    }

    public RotationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}