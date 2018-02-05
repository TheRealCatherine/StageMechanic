public interface IHierarchical
{
    /// <summary>
    /// Object that is the parent of this object. Should be null if this object
    /// does not have a parent.
    /// </summary>
    /// <exception cref="HierarchyException">
    /// Implementations may choose to throw a HierarchyException in the case that an invalid
    /// object (according to implmentation rules) is being set as the Parent of this object.
    /// </exception>
    IHierarchical Parent
    {
        get;
        set;
    }

    /// <summary>
    /// Implementations should return a copy of the list of children of this object.
    /// </summary>
    IHierarchical[] Children
    {
        get;
    }

    /// <summary>
    /// Attempts to add a child to this object.
    /// </summary>
    /// <param name="child"></param>
    /// <exception cref="HierarchyException">
    /// Implementations may choose to throw a HierarchyException in the case
    /// that the child cannot be added to this object.
    /// </exception>
    void AddChild(IHierarchical child);

    /// <summary>
    /// Attempts to remove a child from thie object.
    /// </summary>
    /// <param name="child"></param>
    /// <exception cref="HierarchyException">
    /// Implementations may throw a HierarchyException in the cast that the
    /// child is not present in the 
    /// </exception>
    void RemoveChild(IHierarchical child);
}