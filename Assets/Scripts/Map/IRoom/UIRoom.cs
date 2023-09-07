using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIRoom : MonoBehaviour
{
    protected Room m_baseRoom;
    
    /// <summary>
    /// 초기 데이터 설정
    /// </summary>
    public virtual void Init(Room _baseRoom)
    {
        m_baseRoom = _baseRoom;
    }

    /// <summary>
    /// 플레이어가 입장할 때, 클리어되지 않은 맵일 때 실행
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Execute() 후, 클리어 조건 달성 시 실행
    /// </summary>
    protected virtual void End()
    {
        GetComponent<BaseRoom>().IsClear = true;
    }
}
