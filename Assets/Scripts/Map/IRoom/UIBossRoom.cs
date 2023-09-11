using System.Collections;
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