using System.Collections.Generic;
using UnityEngine;

public class UINormalRoom : MonoBehaviour, IRoom
{
    private NormalRoom m_mRoom;
    private List<Transform> m_spawnPositions;
    
    public void Init(Room _baseRoom)
    {
        m_mRoom = _baseRoom as NormalRoom;

        var spawnPosParentTr = GetComponent<BaseRoom>().m_monsterSpawnPositions;
        m_spawnPositions = new();
        for (int i = 0, cnt = spawnPosParentTr.childCount; i < cnt; i++)
        {
            m_spawnPositions.Add(spawnPosParentTr.GetChild(i));
        }
    }

    public void Execute()
    {
        var spawnMonsters = m_mRoom.m_monsters;
        int spawnPosIdx = 0;
        for (int i = 0, cnt = m_mRoom.m_monsters.Count; i < cnt; i++)
        {
            GameObject monsterPrefab = null; //  spawnMonsters[i]:Monster 정보를 기반으로 프리팹 생성
            Instantiate(monsterPrefab, m_spawnPositions[spawnPosIdx]);
                
            // 다음 위치에 생성
            spawnPosIdx++;
            spawnPosIdx %= m_spawnPositions.Count;
        }
    }
    
    /// <summary>
    /// [TODO] 몬스터 쪽에서 죽을 때, 해당 함수를 호출해주어야 함
    /// </summary>
    public void KillMonsterCount()
    {

        if (false) // [TODO] Monster Count == 0 
        {
            End();
        }
    }

    public void End()
    {
        
    }
}