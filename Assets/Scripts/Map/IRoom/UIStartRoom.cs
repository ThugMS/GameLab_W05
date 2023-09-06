
using System.Collections;
using UnityEngine;

public class UIStartRoom : MonoBehaviour, IRoom
{
    public void Init(Room _baseRoom)
    {        
        var start = _baseRoom as StartRoom;
    }

    public void Execute()
    {
        StartCoroutine(TempWait());
    }
    
    IEnumerator TempWait()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<UIRoom>().IsClear = true;
    }

    public void End()
    {
        
    }
}