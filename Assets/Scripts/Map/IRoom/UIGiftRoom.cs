using Unity.Mathematics;
using UnityEngine;

public class UIGiftRoom : UIRoom
{
    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
    }

    public override void Execute()
    {
        var gift = TestSample.Instance.m_reward;
        Instantiate(gift, transform.position, quaternion.identity);
        End();
    }

    protected override void End()
    {
        base.End();
    }
}