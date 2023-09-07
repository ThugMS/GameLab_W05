
using System.Collections;
using UnityEngine;

public class UIStartRoom : UIRoom
{
    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
    }

    public override void Execute()
    {
        StartCoroutine(TempWait());
    }
    
    IEnumerator TempWait()
    {
        yield return new WaitForSeconds(5f);
        End();
    }

    protected override void End()
    {
        base.End();
    }
}