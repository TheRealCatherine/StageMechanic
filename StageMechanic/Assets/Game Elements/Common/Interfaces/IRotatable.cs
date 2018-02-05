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

    /// <summary>
    /// This should only be true if the object cannot be rotated by any means, even
    /// if everything around it is rotating. This is not expected to be a commonly
    /// used feature in any anticipated sitations, but implementors should take
    /// it into account.
    /// </summary>
    bool IsFixedRotation
    {
        get;
        set;
    }
}