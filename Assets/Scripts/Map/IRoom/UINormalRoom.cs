
using System.Collections.Generic;
using Unity.VisualScripting;
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
        MonsterType monsterType = MonsterType.melee;

        ResourceManager.Instance.MonsterPrefabDict.TryGetValue(monsterType, out List<GameObject> targetMonsters);

        RoomManager roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();

        int monsterCount = Random.Range(roomManager.minMonsterCount, roomManager.maxMonsterCount);

        if(targetMonsters == null)
        {
            Debug.Log(monsterType + "has no prefab/Failed to gete ResourceManager's list");
        }

        int spawnPosIdx = 0;
        for (int i = 0, cnt = monsterCount; i < cnt; i++)
        {
            var obj = Instantiate(getRandomMonster(targetMonsters), m_spawnPositions[spawnPosIdx].position, Quaternion.identity);
            obj.GetComponent<BaseMonster>().DeadListener = KillMonsterCount;
            spawnPosIdx++;
            spawnPosIdx %= m_spawnPositions.Count;
        }

        m_monsterCount = monsterCount;
        
    }

    public GameObject getRandomMonster(List<GameObject> monsterList)
    {
        while(true)
        {
            int index = Random.Range(0, monsterList.Count);
            GameObject tempMonster = monsterList[index];
            /*
            if(tempMonster의 level이 원하는 만큼) TODO : maxLevel 변수 주세용
            */

            return tempMonster;
        }
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

/*
       foreach (var monster in existingMonsters)
        {
            BaseMonster monsterComponent = monster.GetComponent<BaseMonster>();
            monsterComponent.DeadListener = KillMonsterCount;
            monsterComponent.init();
        }
 */