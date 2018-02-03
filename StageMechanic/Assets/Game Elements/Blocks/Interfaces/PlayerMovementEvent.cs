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
