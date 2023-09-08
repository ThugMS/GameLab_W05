using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    #region PublicVariables
    [Header("External Connection")]
    public Transform player;// 샘플
    #endregion
    
    #region PrivateVariables
    [Header("Room-related Information")]
    [SerializeField] private Room[,] m_rooms;
    [SerializeField] private BaseRoom[,] m_uiRooms;
    [SerializeField] private GameObject m_roomPrefabs;
    [SerializeField] private Transform m_gridTr;
    private BaseRoom m_StartBaseRoom;
    private bakeRuntime m_bakeRuntime;
    
    [Header("Room Prefab Tile Size")]
    [SerializeField] private int m_roomWidthSize;
    [SerializeField] private int m_roomHeightSize;
    
    [Header("Room Setting")] 
    [SerializeField] private int m_roomCount;

    [Header("Normal Map Setting")]
    [SerializeField] public int minMonsterLevel;
    [SerializeField] public int maxMonsterLevel;
    [SerializeField] public int minMonsterCount;
    [SerializeField] public int maxMonsterCount;
    #endregion


    #region PublicMethod
    public void Init(int _roomCount)
    {
        // Sample();
        
        GenerateRandomMapTree(_roomCount,
                              GameManager.Instance.m_keywordRoomType);
        
        GenerateRoom();
        InitPlayerPosition();
        m_bakeRuntime.updateMesh();
    }
    
    /// <summary>
    /// 방 -> 방 이동
    /// </summary>
    /// <param name="leavedBaseRoom">직전 방</param>
    /// <param name="leavedDirection">직전 방에서 들어간 문의 방향</param>
    public void MovePlayer(BaseRoom leavedBaseRoom, Direction leavedDirection)
    {
        BaseRoom nextBaseRoom = GetVisitRoom(leavedBaseRoom, leavedDirection);
        var nextDirection = GetOppositeDirection(leavedDirection);
        nextBaseRoom.VisitRoom(player, nextDirection);
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
        m_bakeRuntime = GetComponentInChildren<bakeRuntime>();
        Init(m_roomCount); // [TODO] 이후 외부에서 호출되도록 수정
    }
    
    #region Create Random Map

    private int mm_roomCount;
    /// <summary>
    /// 랜덤 맵 생성
    /// </summary>
    Room GenerateRandomMapTree(int _roomCount, RoomType _specialRoomType)
    {
        Room startRoom = new() {Type = RoomType.Start};
        startRoom.Depth = 0;
        
        int startRoomChildCount = Random.Range(2, 4);
        mm_roomCount = _roomCount - ( 1 + startRoomChildCount);
        
        List<Room> list = new(){ startRoom }; 
        MakeRoom(startRoom, startRoomChildCount, list);
        SetRoomType(list, _specialRoomType);
        GenerateRandomMapArray(list);
        
        return startRoom;
    }
    
    void MakeRoom(Room _parentRoom, int _childRoomCount, List<Room> _list)
    {
        // 현재 방과 연결할 방들 설정
        for (int i = 0; i < _childRoomCount; i++)
        {
            var room = new Room();
            room.Depth = _parentRoom.Depth + 1;
            
            _list.Add(room);
            _parentRoom.ChildRoom.Add(room);
        }

        // 자식 방과 연결할 방을 생성 및 연결
        for (int i = 0; i < _childRoomCount; i++)
        {
            if (mm_roomCount > 0)
            {
                int cnt;
                if (i == _childRoomCount - 1)
                {
                    cnt = Mathf.Min(mm_roomCount, 3);
                }
                else
                {
                    cnt = Mathf.Min(Random.Range(1, mm_roomCount), 3);
                }

                if (cnt > 0)
                {
                    mm_roomCount -= cnt;
                    MakeRoom(_parentRoom.ChildRoom[i], cnt, _list);
                }
            }
        }
    }

    void SetRoomType(List<Room> _rooms, RoomType _specialRoomType)
    {
        // 룸 배치
        int maxDepth = _rooms.Max(x => x.Depth);

        // 보스 룸 설정
        var maxDepthRooms = _rooms.Where(x => x.Depth == maxDepth).ToList();
        int bossIdx = Random.Range(0, maxDepthRooms.Count());
        maxDepthRooms[bossIdx].Type = RoomType.Boss;
        
        // 특수 맵 설정
        var ignoreRooms = _rooms.Where(x => x.Type == RoomType.Ignore).ToList();
        int specialRoomCount =  _rooms.Count / 5; // [TODO] 5개 방당 1개 꼴로 설정. RoomManager 머지 후 해당 값 수정 필요
        for (int i = 0; i < specialRoomCount; i++)
        {
            int idx = Random.Range(0, ignoreRooms.Count);
            ignoreRooms[idx].Type = _specialRoomType;
            ignoreRooms.RemoveAt(idx);
        }
        
        // 기본 맵 설정
        foreach (var room in _rooms.Where(x=> x.Type == RoomType.Ignore))
        {
            room.Type = RoomType.Normal;
        }
    }

    void GenerateRandomMapArray(List<Room> _rooms)
    {
        int maxDepth = _rooms.Max(x => x.Depth);
        int size = _rooms.Count();
        
        var way = new(int x, int y)[4] {(0, 1), (0, -1), (-1, 0), (1, 0)};
        int pivot = size / 2;
        int depth = 1;
        
        m_rooms = new Room[size, size];
        
        // 첫 방 설정
        var startRoom = _rooms.First();
        startRoom.m_grid = new(pivot, pivot);
        m_rooms[pivot, pivot] = startRoom;

        // 큐 준비
        var parents = new List<Room>() { startRoom };
        var nextParents = new List<Room>();
        var q = new Queue<Room>(_rooms.Where(x => x.Depth == 1).ToList());
        while (q.Count > 0)
        {
            var item = q.Dequeue();
            nextParents.Add(item);

            bool canExit;
            do
            {
                // 랜덤 부모
                int parentIdx = Random.Range(0, parents.Count);
                var curParent = parents[parentIdx];
                List<int> canDirIdxs = new();
                for (int i = 0; i < 4; i++)
                {
                    var vec =  curParent.m_grid;
                    var targetPos = new Vector2Int(vec.x+ way[i].x,  vec.y + way[i].y);

                    if (targetPos.x >= 0 && targetPos.x < size && targetPos.y >= 0 && targetPos.y < size
                        && m_rooms[targetPos.y, targetPos.x] == null)
                    {
                        canDirIdxs.Add(i);
                    }
                }

                if (canDirIdxs.Count > 0)
                {
                    //랜덤 방향
                    var randomDiridx = Random.Range(0, canDirIdxs.Count);
                    int dirIdx = canDirIdxs[randomDiridx];
                    Direction direction = (Direction)(dirIdx+1);
                    
                    // 부모에 자식 방향 지정
                    curParent.HasDirectionList.Add(direction);
                    
                    // 자식에서 부모 방향 지정
                    var opos = GetOppositeDirection(direction);
                    item.HasDirectionList.Add(opos);

                    // 배열에 차지
                    var vec =  curParent.m_grid + new Vector2Int(way[dirIdx].x, way[dirIdx].y);
                    m_rooms[vec.y, vec.x] = item;
                    item.m_grid = vec;
                    
                    canExit = false;
                }
                else
                {
                    canExit = true;
                }
            } while (canExit);

            
            if (q.Count == 0)
            {
                if (depth <= maxDepth)
                {
                    depth++;
                    q = new Queue<Room>(_rooms.Where(x => x.Depth == depth).ToList());

                    parents = new(nextParents);
                    nextParents = new();
                }
            }
        }
    }

    List<(int y, int x)> GetCanDirection()
    {
        var list = new List<(int y, int x)>();
        var res = new List<(int y, int x)>();
        
        
        foreach (var item in list)
        {
        }
        

        return list;
    }
    #endregion

    /// <summary>
    /// 지정된 배열 정보로 맵을 생성
    /// </summary>
    void GenerateRoom()
    {
        m_uiRooms = new BaseRoom[m_rooms.GetLength(0), m_rooms.GetLength(1)];
        
        for (int y = 0, maxY = m_rooms.GetLength(0); y < maxY; y++)
        {
            for (int x = 0, maxX = m_rooms.GetLength(1); x < maxX; x++)
            {
                var room = m_rooms[y, x]; 
                if (room != null)
                {
                    var obj = Instantiate(m_roomPrefabs, m_gridTr);
                    obj.transform.position = new Vector3(x * m_roomWidthSize, y * m_roomHeightSize, 0);

                    var roomByType = CreateUIRoomByType(obj, room.Type);
                    roomByType.Init(room);
                    
                    var uiRoom = obj.GetComponent<BaseRoom>();
                    m_uiRooms[y, x] = uiRoom;
                    uiRoom.m_grid = new Vector2Int(y, x);
                    uiRoom.Init(this, roomByType, GetRoomTypes(room));

                    if (room.Type == RoomType.Start)
                    {
                        m_StartBaseRoom = uiRoom;
                    }
                }
            }
        }
    }

    RoomType[] GetRoomTypes(int _y, int _x)
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
    
    
    RoomType[] GetRoomTypes(Room _room)
    {
        var types = new RoomType[4];

        int[] wayX = new[] { 0, 0, -1, 1 };
        int[] wayY = new[] { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            Direction direction = (Direction) i + 1;
            
            if (_room.HasDirectionList.Contains(direction))
            {
                var nextRoom = m_rooms[_room.m_grid.y + wayY[i], _room.m_grid.x + wayX[i]];
                types[i] = nextRoom.Type;
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
        m_StartBaseRoom.VisitRoom(player);
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

    BaseRoom GetVisitRoom(BaseRoom currentBaseRoom, Direction _leaveDirection)
    {
        Vector2Int vec = currentBaseRoom.m_grid;

        var nextVector =  _leaveDirection switch
        {
            Direction.Up    => new Vector2Int(vec.x + 1, vec.y),
            Direction.Down  => new Vector2Int(vec.x - 1, vec.y),
            Direction.Left  => new Vector2Int(vec.x, vec.y - 1),
            Direction.Right => new Vector2Int(vec.x, vec.y + 1),
        };

        BaseRoom visitBaseRoom = m_uiRooms[nextVector.x, nextVector.y];

        return visitBaseRoom;
    }

    #region Factory

    public UIRoom CreateUIRoomByType(GameObject _obj, RoomType _type)
    {
        return _type switch
        {
            RoomType.Start       => _obj.AddComponent<UIStartRoom>(),
            RoomType.Normal      => _obj.AddComponent<UINormalRoom>(),
            RoomType.NormalGift  => _obj.AddComponent<UINormalRoom>(),
            RoomType.Gift        => _obj.AddComponent<UIGiftRoom>(),
            RoomType.Boss        => _obj.AddComponent<UIBossRoom>(),
        };
    }

    #endregion
    #endregion
}
