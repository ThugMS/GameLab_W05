using System.Collections.Generic;

public class Room
{
    public RoomType Type { get; private set; }
    
    public List<Monster> m_monsters;
    
    public Boss m_boss;

    public Gift m_gift;

    public Room(RoomType type)
    {
        Type = type;
    }
} 