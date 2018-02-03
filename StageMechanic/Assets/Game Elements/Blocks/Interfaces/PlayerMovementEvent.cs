/*  
 * Copyright (C) Catherine. All rights reserved.  
 * Licensed under the BSD 3-Clause License.
 * See LICENSE file in the project root for full license information.
 * See CONTRIBUTORS file in the project root for full list of contributors.
 */
 public class PlayerMovementEvent
{
    public enum EventType
    {
        None = 0,
        Enter,
        Stay,
        Leave
    }

    public enum EventLocation
    {
        None = 0,
        Top,
        Side,
        Bottom
    }

    public EventType Type = EventType.None;
    public IPlayerCharacter Player;
    public EventLocation Location = EventLocation.None;
}
