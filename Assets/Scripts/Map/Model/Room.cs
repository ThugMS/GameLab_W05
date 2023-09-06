public class Room
{
    public RoomType Type { get; private set; }

    public Room(RoomType type)
    {
        Type = type;
    }
}