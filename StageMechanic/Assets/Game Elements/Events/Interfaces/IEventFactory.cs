/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
using UnityEngine;

public interface IEventFactory {

    IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, int eventTypeIndex, GameObject parent = null);
    IEvent CreateEvent(Vector3 globalPosition, Quaternion globalRotation, string eventTypeName, GameObject parent = null);
    IEvent CreateEvent(int eventTypeIndex, IBlock parent);
    IEvent CreateEvent(string eventTypeName, IBlock parent);

}
