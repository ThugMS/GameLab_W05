using UnityEngine;

public class UIBossRoom : UIRoom
{
    private Boss m_boss;

    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
        m_boss = _baseRoom.m_boss;
    }

    public override void Execute()
    {
        // 코루틴해서 서사를 보이든.. .
        var obj = TestSample.Instance.m_boss; //[TODO] 보스 정보에 따른 GameObject를 생성
        var boss = Instantiate(obj, transform.position, Quaternion.identity);
        
        boss.GetComponent<BaseMonster>().DeadListener = End;
    }

    protected override void End()
    {
        base.End();
        GameManager.Instance.GameClear();
    }
}