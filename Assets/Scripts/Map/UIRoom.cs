using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Ignore,
    Up = 1,
    Down,
    Left,
    Right
}

public class UIRoom : MonoBehaviour
{
    private RoomManager m_roomManager;
    private Room m_baseRoom;
    
    public Vector2Int grid;

    [Header("위치 정보")]
    [SerializeField] public RoomDoor[] m_doors;
    [SerializeField] public Transform m_monsterSpawnPositions;


    [Header("방 구성")]
    [SerializeField] private GameObject m_desactiveAllDoorObj;

    private bool m_isInit;
    private bool m_isClear;
    public bool IsClear
    {
        get => m_isClear;
        set
        {
            m_isClear = value;
            if (m_isClear) OpenDoor();
            else CloseDoor();
        }
    }
    
    /// <summary>
    /// 방 정보를 초기화
    /// 지정된 정보에 맞게 문 및 생성
    /// </summary>
    public void Init(Room baseRoom)
    {
        m_baseRoom = baseRoom;
    }
    
    void OpenDoor()
    {
        
    }
    
    void CloseDoor()
    {
        
    }

    public void VisitRoom(Transform playerTr, Direction _direction)
    {
        playerTr.position = GetDirectionTr(_direction).position;
    }

    public void LeavedRoom(Direction _direction)
    {
        
    }


    Transform GetDirectionTr(Direction direction)
    {
        return direction switch
        {
            Direction.Up    => m_doors[0].spawnPosition,
            Direction.Down  => m_doors[1].spawnPosition,
            Direction.Left  => m_doors[2].spawnPosition,
            Direction.Right => m_doors[3].spawnPosition,
        };
    }
}
