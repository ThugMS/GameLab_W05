using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    #region PublicVariables
    [Header("External Connection")]
    public Transform player;// 샘플
    #endregion
    
    #region PrivateVariables
    [Header("Room-related Information")]
    [SerializeField] private Room[,] m_rooms;
    [SerializeField] private UIRoom[,] m_uiRooms;
    [SerializeField] private GameObject m_roomPrefabs;
    [SerializeField] private Transform m_gridTr;
    private UIRoom m_startUIRoom;
    
    [Header("Room Prefab Tile Size")]
    [SerializeField] private int m_roomWidthSize;
    [SerializeField] private int m_roomHeightSize;
    #endregion
    
    
    #region PublicMethod
    public void Init()
    {
        Sample();
        GenerateRoom();
        InitPlayerPosition();
    }
    
    /// <summary>
    /// 방 -> 방 이동
    /// </summary>
    /// <param name="leavedUIRoom">직전 방</param>
    /// <param name="leavedDirection">직전 방에서 들어간 문의 방향</param>
    public void MovePlayer(UIRoom leavedUIRoom, Direction leavedDirection)
    {
        var nextDirection = GetOppositeDirection(leavedDirection);
        UIRoom nextUIRoom = GetVisitRoom(leavedUIRoom, nextDirection);
        
        nextUIRoom.VisitRoom(player, nextDirection);
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
    #endregion


    #region PrivateMethod
    private void Start()
    {
        Init(); // [TODO] 이후 외부에서 호출되도록 수정
    }

    void Sample()
    {
        m_rooms = new Room[1, 3]
        {
            {
                new (RoomType.Start),
                new (RoomType.Monster),
                new (RoomType.Boss)
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
                var room = m_rooms[y, x]; 
                if (room != null)
                {
                    var obj = Instantiate(m_roomPrefabs, m_gridTr);
                    obj.transform.position = new Vector3(x * m_roomWidthSize, y * m_roomHeightSize, 0);
                    
                    var uiRoom = obj.GetComponent<UIRoom>();
                    uiRoom.m_grid = new Vector2Int(y, x);
                    uiRoom.Init(this, room, GetRoomType(y, x));

                    if (room.Type == RoomType.Start)
                    {
                        m_startUIRoom = uiRoom;
                    }
                }
            }
        }
    }

    RoomType[] GetRoomType(int _y, int _x)
    {
        var types = new RoomType[4];

        int[] wayX = new[] { 0, 0, -1, 1 };
        int[] wayY = new[] { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            var room = GetValidRoom(_y + wayY[i], _x + wayX[i]);
            if (room != null)
            {
                types[i] = room.Type;
            }
        }

        return types;
    }
    
    bool CheckValidRoom(int _y, int _x)
    {
        bool result = false;
        if (m_rooms.GetLength(0) < _y &&
            m_rooms.GetLength(1) < _x)
        {
            if (m_uiRooms[_y, _x] != null)
            {
                result = true;
            }
        }

        return result;
    }
    
    Room GetValidRoom(int _y, int _x)
    {
        Room room = null;

        if (0 <= _y && _y < m_rooms.GetLength(0) &&
            0 <= _x && _x < m_rooms.GetLength(1))
        {
            room = m_rooms[_y, _x];
        }

        return room;
    }

    /// <summary>
    /// 플레이어의 위치를 초기화
    /// </summary>
    void InitPlayerPosition()
    {
        
    }


    Direction GetOppositeDirection(Direction _direction)
    {
        return _direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
        };
    }

    UIRoom GetVisitRoom(UIRoom _currentUIRoom, Direction _visitDirection)
    {
        Vector2Int vec = _currentUIRoom.m_grid;

        UIRoom visitUIRoom = _visitDirection switch
        {
            Direction.Up    => m_uiRooms[vec.y + 1, vec.x],
            Direction.Down  => m_uiRooms[vec.y - 1, vec.x],
            Direction.Left  => m_uiRooms[vec.y, vec.x - 1],
            Direction.Right => m_uiRooms[vec.y, vec.x + 1],
        };

        return visitUIRoom;
    }
    #endregion
}