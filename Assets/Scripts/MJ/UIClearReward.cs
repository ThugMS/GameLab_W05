using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIClearReward : MonoBehaviour
{
    private ClearReward m_clearReward;

    public void Init(ClearReward clearReward)
    {
        m_clearReward = clearReward;
    }

    public void B_Click()
    {
        GameManager.Instance.SelectClearReward(m_clearReward);
    }
}
