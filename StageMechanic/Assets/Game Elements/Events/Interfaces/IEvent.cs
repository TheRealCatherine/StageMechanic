/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections.Generic;
using UnityEngine;

public enum EventLocationAffinity
{
    Block = 0,
    Position
}

public interface IEvent {

    /// <summary>
    /// A human-readable identifier for the event. Typically implementations
    /// will auto-generate a guid in the case the user has not given a different
    /// name to the event. 
    /// </summary>
    /// <exception cref="EventNameException">
    /// May throw a EventNameException if the caller tries to set an invalid name.
    /// There is no inherent requirement for Names to be unique, however certain
    /// implementations may choose to impose this or other requirements on naming.
    /// </exception>
    string Name
    {
        get;
        set;
    }

    /// <summary>
    /// A string representation of the type of event. Note that this is
    /// used in save files and other places as well as UI.
    /// </summary>
    string TypeName
    {
        get;
    }

    /// <summary>
    /// Defines if an event should "stick to" a block (ie move with it
    /// like an item) or should be stuck to a given coordinate.
    /// </summary>
    EventLocationAffinity LocationAffinity
    {
        get;
        set;
    }

    Vector3 Position
    {
        get;
        set;
    }

    Dictionary<string, string> Properties
    {
        get;
        set;
    }

    EventJsonDelegate GetJsonDelegate();
}
