using System.Collections.Generic;
using UnityEngine;

public class Room
{
    #region Structure
    public Vector2Int m_grid;
    public List<Room> ChildRoom = new();
    public int Depth;
    public List<Direction> HasDirectionList= new();
    #endregion
    public RoomType Type { get; set; }

    public GameObject m_landspace;

    public List<GameObject> m_monsters;

    public Gift m_gift;

}