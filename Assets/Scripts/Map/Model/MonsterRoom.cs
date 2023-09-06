using System.Collections.Generic;

public class MonsterRoom : Room
{
    public List<Monster> m_monsters;
    public MonsterRoom(List<Monster> _monsters) : base(RoomType.Monster)
    {

    }
}