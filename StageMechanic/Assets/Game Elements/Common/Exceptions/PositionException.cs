using System;

public class PositionException : Exception
{
    public PositionException()
    {
    }

    public PositionException(string message)
        : base(message)
    {
    }

    public PositionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}