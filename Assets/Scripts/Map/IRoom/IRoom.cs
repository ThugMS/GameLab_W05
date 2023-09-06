using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom
{
    /// <summary>
    /// 초기 데이터 설정
    /// </summary>
    void Init(Room _baseRoom);

    /// <summary>
    /// 플레이어가 입장할 때, 클리어되지 않은 맵일 때 실행
    /// </summary>
    void Execute();
    
    /// <summary>
    /// Execute() 후, 클리어 조건 달성 시 실행
    /// </summary>
    void End();
}
