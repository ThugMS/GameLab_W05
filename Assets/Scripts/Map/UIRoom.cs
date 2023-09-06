using UnityEngine;

public class UIRoom : MonoBehaviour
{
    #region PublicVaraibles
    public Vector2Int m_grid { get; set; }
    #endregion
    
    #region PrivateVaraibles
    private RoomManager m_roomManager;
    private Room m_baseRoom;
    
    [Header("위치 정보")]
    [SerializeField] private Transform m_monsterSpawnPositions;

    [Header("방 구성")]
    [SerializeField] private RoomDoor[] m_doors;
    [SerializeField] private GameObject m_desactiveAllDoorObj;

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
    #endregion

    #region PublicMethod
    /// <summary>
    /// 방 정보를 초기화
    /// 지정된 정보에 맞게 문 및 생성
    /// </summary>
    public void Init(RoomManager _roomManager, Room _baseRoom, RoomType[] _types)
    {
        m_roomManager = _roomManager;
        m_baseRoom = _baseRoom;

        for (int i = 0; i < 4; i++)
        {
            m_doors[i].Init(this, _types[i]);
        }

        IsClear = true;
    }

    public void VisitRoom(Transform _playerTr, Direction _direction)
    {
        _playerTr.position = GetDirectionTr(_direction).position;
    }

    public void LeaveRoom(Direction _inDirection)
    {
        m_roomManager.MovePlayer(this, _inDirection);
    }
    #endregion

    #region PrivateMethod
    void OpenDoor()
    {
        m_desactiveAllDoorObj.SetActive(false);
    }
    
    void CloseDoor()
    {
        m_desactiveAllDoorObj.SetActive(true);
    }


    Transform GetDirectionTr(Direction direction)
    {
        return direction switch
        {
            Direction.Up    => m_doors[0].m_spawnPosition,
            Direction.Down  => m_doors[1].m_spawnPosition,
            Direction.Left  => m_doors[2].m_spawnPosition,
            Direction.Right => m_doors[3].m_spawnPosition,
        };
    }
    #endregion
}
