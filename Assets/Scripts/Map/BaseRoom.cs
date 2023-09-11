using System;
using System.Collections.Generic;
using System.Linq;
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
    private Room m_baseRoom;

    [Header("방 구성")]
    [SerializeField] private Transform m_floor;
    [SerializeField] private Transform[] m_spawnPositons;
    [SerializeField] private GameObject m_CloseDoorObject; 
    private RoomDoor[] m_doors;

    private bool m_isClear;
    public bool IsClear
    {
        get => m_isClear;
        set
        {
            m_isClear = value;
            m_CloseDoorObject.SetActive(!m_isClear);
            if (m_isClear == true)
            {
                foreach (var door in m_doors)
                {
                    door.OpenDoorAnime();
                }
            }
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
        m_baseRoom = _room;

        if (_room.m_landspace != null)
        {
            Instantiate(_room.m_landspace, m_floor);
        }
        else throw new Exception($"{_room.Type}타입에 해당하는 landscape가 존재하지 않음");

        m_doors = new RoomDoor[4];
        for (int i = 0; i < 4; i++)
        {
            var direction = (Direction)( i + 1 );
            var resourceRoomType = _types[i]; // Door tile of start type is same to normal door 
            if (resourceRoomType == RoomType.Start) resourceRoomType = RoomType.Normal;
            
            if (ResourceManager.Instance.DoorPrefabDict.ContainsKey((resourceRoomType, direction)))
            {
                var door =  ResourceManager.Instance.DoorPrefabDict[(resourceRoomType, direction)];
                var obj = Instantiate(door, m_floor);
                m_doors[i] = obj.GetComponent<RoomDoor>();
                m_doors[i].Init(this, _types[i], direction);
            }
            else
            {
                Debug.Log($"{_types[i]} 타입의 {direction} 방향 프리팹 정보를 찾을 수 없음");
            }
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
            var moveTransform = m_spawnPositons[((int)_direction) - 1];
            _playerTr.position = moveTransform.position;
            
        }
        else
        {
            _playerTr.position = transform.position + new Vector3(0, -2.5f, 0);
        }

        // 클리어가 되지 않은 맵의 경우, 맵 특성에 맞게 동작 수행
        if (IsClear == false)
        {
            m_UIRoomByType.Execute();
            if (m_baseRoom.Type != RoomType.Gift)
            {
                foreach (var door in m_doors)
                {
                    door.CloseDoorAnime();
                }
            }
        }

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        var cameraPos = transform.position;
        cameraPos.x += .5f;
        cameraPos.y += .5f;
        cameraPos.z = -10;

        Camera.main.transform.position = cameraPos;
    }

    public void LeaveRoom(Direction _inDirection)
    {
        m_roomManager.MovePlayer(this, _inDirection);
    }
    #endregion
}
