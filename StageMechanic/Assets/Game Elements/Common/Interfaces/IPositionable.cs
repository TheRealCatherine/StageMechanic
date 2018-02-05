using UnityEngine;

/// <summary>
/// Describes a class that can be have a Vector3 position in a scene
/// or other representation (which may or may not be graphical
/// depending on implementation).
/// </summary>
public interface IPositionable
{
    /// <summary>
    /// The position of the object in global coordinates.
    /// </summary>
    /// <exception cref="PositionException">
    /// Implementations may choose to throw a PositionException
    /// if the given position is invalid according to whatever
    /// rules the implementation imposes.
    /// </exception>
    Vector3 Position
    {
        get;
        set;
    }
}