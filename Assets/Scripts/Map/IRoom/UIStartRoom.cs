
using System.Collections;
using UnityEngine;

public class UIStartRoom : UIRoom
{
    [SerializeField] private GameObject m_selectedClassObject;
    
    public override void Init(Room _baseRoom)
    {
        base.Init(_baseRoom);
    }

    public override void Execute()
    {
        m_selectedClassObject = transform.Find("../Room(Clone)/Floor/Floor(Clone)/ChoosePlayerClass").gameObject;
    }

    public void CompleteSelect()
    {
        m_selectedClassObject.SetActive(false);
        End();
    }

    protected override void End()
    {
        base.End();
    }
}