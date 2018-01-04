/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent {

    /// <summary>
    /// A human-readable identifier for the event. Typically implementations
    /// will auto-generate a guid in the case the user has not given a different
    /// name to the block. 
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
    /// used in save files and other places as well as UI. Setting this value
    /// should change the type of the event to the specified type.
    /// </summary>
    /// <exception cref="EventTypeExcpetion">
    /// May throw a EventTypeException if the caller tries to set an invalid
    /// event type. Implentations may instead choose to handle this situation
    /// by setting the type to a default value or creating a new event type.
    /// </exception>
    string TypeName
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
