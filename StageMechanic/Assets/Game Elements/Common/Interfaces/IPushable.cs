using UnityEngine;

public interface IPushable
{
    /// <summary>
    /// Determines whether this object can be moved the specified direction distance.
    /// </summary>
    /// <returns><c>true</c> if this object can be moved the specified direction and distance; otherwise, <c>false</c>.</returns>
    /// <param name="direction">A Vector3 describing the global direction such as <c>Vector3.left</c></param>
    /// <param name="distance">Distance in Unity units.</param>
    bool CanBePushed(Vector3 direction, int distance = 1);

    /// <summary>
    /// Move the specified direction and distance.
    /// </summary>
    /// <returns><c>true</c> if this object will be moved the specified direction and distance; otherwise, <c>false</c>.</returns>
    /// <param name="direction">A Vector3 describing the global direction such as <c>Vector3.left</c></param>
    /// <param name="distance">Distance in Unity units.</param>
    bool Push(Vector3 direction, int distance = 1);

    /// <summary>
    /// Returns a multiplier indicating how heavy or light the block is which, for example, can be used to time animations
    /// involving moving the object. 1.0 should indicate normal weight, 0.5 is half-weight, 2.0 is double-weight
    /// and 0.0 indicates that the object cannot be moved.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    float PushWeight(Vector3 direction, int distance = 1);
}