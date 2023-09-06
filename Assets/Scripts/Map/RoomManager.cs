using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room-related Information")]
    [SerializeField] private Room[,] m_rooms;
    [SerializeField] private UIRoom[,] m_uiRooms;
    [SerializeField] private GameObject m_roomPrefabs;
    [SerializeField] private Transform m_gridTr;
    
    [SerializeField] private int m_roomWidthSize;
    [SerializeField] private int m_roomHeightSize;
    
    public UIRoom StartUIRoom { get; private set; }

    public Transform player;// 샘플
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Sample();
        GenerateRoom();
    }

    void Sample()
    {
        m_rooms = new Room[1, 3]
        {
            {
                new Room(RoomType.Start),
                new Room(RoomType.Monster),
                new Room(RoomType.Boss)
            },
        }; 
    }
    
    /// <summary>
    /// 랜덤 맵 생성
    /// </summary>
    void GenerateRandomMap() //[TODO] 추후 맵 타입 입력 받기
    {
            
    }

    /// <summary>
    /// 지정된 배열 정보로 맵을 생성
    /// </summary>
    void GenerateRoom()
    {
        for (int y = 0, maxY = m_rooms.GetLength(0); y < maxY; y++)
        {
            for (int x = 0, maxX = m_rooms.GetLength(1); x < maxX; x++)
            {
                if (m_rooms[y, x] != null)
                {
                    var obj = Instantiate(m_roomPrefabs, m_gridTr);
                    obj.transform.position = new Vector3(x * m_roomWidthSize, y * m_roomHeightSize, 0);
                    
                    var room = obj.GetComponent<UIRoom>();
                    room.grid = new Vector2Int(y, x);
                    room.Init(m_rooms[y, x]);
                }
            }
        }
    }

    UIRoom GetValidRoom(int _x, int _y)
    {
        UIRoom uiRoom = null;

        if (m_rooms.GetLength(0) < _y &&
            m_rooms.GetLength(1) < _x)
        {
            uiRoom = m_uiRooms[_y, _x];
        }

        return uiRoom;
    }

    /// <summary>
    /// 플레이어의 위치를 초기화
    /// </summary>
    void InitPlayerPosition()
    {
        
    }

    public void MovePlayer(UIRoom leavedUIRoom, Direction leavedDirection)
    {
        var nextDirection = GetOppositeDirection(leavedDirection);
        UIRoom nextUIRoom = GetVisitRoom(leavedUIRoom, nextDirection);
        
        nextUIRoom.VisitRoom(player, nextDirection);
    }

    Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
        };
    }

    UIRoom GetVisitRoom(UIRoom currentUIRoom, Direction visitDirection)
    {
        Vector2Int vec = currentUIRoom.grid;

        UIRoom visitUIRoom = visitDirection switch
        {
            Direction.Up    => m_uiRooms[vec.y + 1, vec.x],
            Direction.Down  => m_uiRooms[vec.y - 1, vec.x],
            Direction.Left  => m_uiRooms[vec.y, vec.x - 1],
            Direction.Right => m_uiRooms[vec.y, vec.x + 1],
        };

        return visitUIRoom;
    }
    

    public void ClearMap()
    {
        foreach (var room in m_uiRooms)
        {
            if (room != null)
            {
                Destroy(room.gameObject);
            } 
        }
    }
}
