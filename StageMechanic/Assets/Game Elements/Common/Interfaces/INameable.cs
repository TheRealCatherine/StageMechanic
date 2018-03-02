/// <summary>
/// This interface is used for objects that have a name assigned
/// to the object. The name may be unique or non-unique depending
/// on the implementation. Commonly this be a random GUID unless
/// otherwise specified, but this may vary by implementation.
/// </summary>
public interface INameable {

    /// <summary>
    /// Name of the object instance.
    /// </summary>
    /// <exception cref="NameException">
    /// Implementations may opt to throw an NameException
    /// if setting an invalid name. This can be used, for example,
    /// in the case that an implementation requires unique names
    /// but a duplicate is provided, or in the case that the supplied
    /// name does not meet some other naming requirement.
    /// </exception>
    string Name
    {
        get;
        set;
    }
}
