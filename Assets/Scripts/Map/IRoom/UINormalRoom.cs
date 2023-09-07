using System.Collections.Generic;
using UnityEngine;

public class UINormalRoom : UIRoom
{
    private List<Transform> m_spawnPositions;
    private int m_monsterCount;
    
    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
        
        var spawnPosParentTr = GetComponent<BaseRoom>().m_monsterSpawnPositions;
        m_spawnPositions = new();
        for (int i = 0, cnt = spawnPosParentTr.childCount; i < cnt; i++)
        {
            m_spawnPositions.Add(spawnPosParentTr.GetChild(i));
        }
    }

    public override void Execute()
    {
        var spawnMonsters = m_baseRoom.m_monsters;
        int spawnPosIdx = 0;
        for (int i = 0, cnt = 4; i < cnt; i++)// m_mRoom.m_monsters.Count; i < cnt; i++) //  [TODO] 지정된 몬스터 수로 수정 필요
        {
            GameObject monsterPrefab = TestSample.Instance.m_monsterPrefab[0]; //  [TODO] spawnMonsters[i]:Monster 정보를 기반으로 프리팹 생성
            var obj = Instantiate(monsterPrefab, m_spawnPositions[spawnPosIdx].position, Quaternion.identity);
            obj.GetComponent<BaseMonster>().DeadListener = KillMonsterCount;
            
            // 다음 위치에 생성
            spawnPosIdx++;
            spawnPosIdx %= m_spawnPositions.Count;
        }

        m_monsterCount = 4; // [TODO] m_mRoom.monsters.Count;

    }
    
    /// <summary>
    /// 몬스터 쪽에서 죽을 때, 해당 함수를 호출해주어야 함
    /// </summary>
    void KillMonsterCount()
    {
        m_monsterCount--;
        if (m_monsterCount == 0) 
        {
            End();
        }
    }

    protected override void End()
    {
        base.End();

        if (m_baseRoom.Type == RoomType.NormalGift)
        {
            var gift = TestSample.Instance.m_reward;
            Instantiate(gift, transform.position, Quaternion.identity);
        }
    }
}