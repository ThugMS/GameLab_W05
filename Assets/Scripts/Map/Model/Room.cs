using System.Collections.Generic;
using UnityEngine;

public class Room
{
    #region Structure
    public Vector2Int m_grid;
    public Room ParentRoom;
    public List<Room> ChildRoom = new();
    public int Depth;
    public List<Direction> HasDirectionList= new();
    #endregion
    public RoomType Type { get; set; }

    public List<GameObject> m_monsters;
    
    public Boss m_boss;

    public Gift m_gift;

    public Room(Room _parent)
    {
        ParentRoom = _parent;
    }
    public Room(RoomType type)
    {
        Type = type;
    }
}