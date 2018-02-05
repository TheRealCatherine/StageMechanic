using UnityEngine;

/// <summary>
/// Used to describe objects which can have a rotation describable
/// by a Quaternion value.
/// </summary>
public interface IRotatable
{
    /// <summary>
    /// Global rotation of the object.
    /// </summary>
    /// <exception cref="RotationException">
    /// Implementations may choose to throw a RotationException if
    /// an invalid, according to implementation-specific criteria,
    /// rotation is applied.
    /// </exception>
    Quaternion Rotation
    {
        get;
        set;
    }
}