using System.Collections.Generic;

public class NormalRoom : Room
{
    public List<Monster> m_monsters;
    
    public NormalRoom(List<Monster> _monsters) : base(RoomType.Normal)
    {
        m_monsters = _monsters;
    }
}