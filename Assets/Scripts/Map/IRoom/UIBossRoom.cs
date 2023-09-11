using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossRoom : UIRoom
{
    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
    }

    public override void Execute()
    {
        var obj = Instantiate(ResourceManager.Instance.GetBossByType(GameManager.Instance.m_keywordMonsterType), 
            transform.position,
            Quaternion.identity);
        var boss =  obj.GetComponent<BaseMonster>();
        var hpList = new List<int>() { 50, 70, 100 }; 
        boss.Health = hpList[GameManager.Instance.m_currentStage - 1];
        boss.DeadListener = End;
    }

    protected override void End()
    {
        // base.End();
        StartCoroutine(ClearProcess());
    }

    IEnumerator ClearProcess()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GameClear();
    }
}