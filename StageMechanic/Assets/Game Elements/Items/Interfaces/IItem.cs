using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{

    /// <summary>
    /// A human-readable identifier for the item. Typically implementations
    /// will auto-generate a guid in the case the user has not given a different
    /// name to the item. 
    /// </summary>
    /// <exception cref="ItemNameException">
    /// May throw a BlockNameException if the caller tries to set an invalid name.
    /// There is no inherent requirement for Names to be unique, however certain
    /// implementations may choose to impose this or other requirements on naming.
    /// </exception>
    string Name
    {
        get;
        set;
    }

    /// <summary>
    /// A string representation of the type of item. Note that this is
    /// used in save files and other places as well as UI. Setting this value
    /// should change the type of the item to the specified type.
    /// </summary>
    /// <exception cref="ItemTypeExcpetion">
    /// May throw a ItemTypeException if the caller tries to set an invalid
    /// item type. Implentations may instead choose to handle this situation
    /// by setting the type to a default value or creating a new item type.
    /// </exception>
    string TypeName
    {
        get;
        set;
    }

    /// <summary>
    /// Determines if the player can collect this item or if it should remain
    /// after the player makes contact with it.
    /// </summary>
	bool Collectable { get; set; }

    /// <summary>
    /// How much score should the player gain or lose by collecting this item?
    /// </summary>
    long Score { get; set; }


}
