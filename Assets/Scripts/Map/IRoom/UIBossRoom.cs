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
        StartCoroutine(ShowEnterBossRoom());
        
        
    }

    IEnumerator ShowEnterBossRoom()
    {
        // 세팅
        var obj = Instantiate(ResourceManager.Instance.GetBossByType(GameManager.Instance.m_keywordMonsterType), 
                                                transform.position,
                                                Quaternion.identity);
        var boss =  obj.GetComponent<BaseMonster>();
        boss.DeadListener = End;
        
        yield return new WaitForSeconds(2f);
        
        // [TODO] Boss Inir?
    }

    protected override void End()
    {
        base.End();
        GameManager.Instance.GameClear();
    }
}