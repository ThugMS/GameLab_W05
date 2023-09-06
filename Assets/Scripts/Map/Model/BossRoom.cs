public class BossRoom : Room
{
    public Boss m_boss;
    public BossRoom(Boss boss) : base(RoomType.Boss)
    {
    }
}