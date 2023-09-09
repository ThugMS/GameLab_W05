using UnityEngine;

/// <summary>
/// 모든 방이 가지는 기본 설정
/// </summary>
public class BaseRoom : MonoBehaviour
{
    #region PublicVaraibles
    public Vector2Int m_grid { get; set; }
    #endregion
    
    #region PrivateVaraibles
    private RoomManager m_roomManager;
    private UIRoom m_UIRoomByType;

    [Header("방 구성")]
    [SerializeField] private Transform m_floor;
    [SerializeField] private RoomDoor[] m_doors;
    [SerializeField] private GameObject m_desactiveAllDoorObj;
    public Transform m_monsterSpawnPositions;

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
    public void Init(RoomManager _roomManager, UIRoom _uiRoomByType, Room _room, RoomType[] _types)
    {
        m_roomManager = _roomManager;
        m_UIRoomByType = _uiRoomByType;

        Instantiate(_room.m_landspace, m_floor);

        for (int i = 0; i < 4; i++)
        {
            m_doors[i].Init(this, _types[i]);
        }

        IsClear = false ;
    }

    /// <summary>
    /// 해당 방으로 이동
    /// 만약, 방향이 입력되지 않았다면 중앙에서 생성
    /// </summary>
    public void VisitRoom(Transform _playerTr, Direction _direction = Direction.Ignore)
    {
        if (_direction != Direction.Ignore)
        {
            var moveTransform = GetDirectionTr(_direction);
            _playerTr.position = moveTransform.position;
            
        }
        else
        {
            _playerTr.position = transform.position;
        }

        // 클리어가 되지 않은 맵의 경우, 맵 특성에 맞게 동작 수행
        if (IsClear == false)
        {
            m_UIRoomByType.Execute();
        }

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        var cameraPos = transform.position;
        cameraPos.z = -10;

        Camera.main.transform.position = cameraPos;
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
