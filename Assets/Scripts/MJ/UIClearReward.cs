using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIClearReward : MonoBehaviour
{
    enum RewardType : int
    {
        Hp = 0,
        Power,
        Speed
    }
    private ClearReward m_clearReward;
    [SerializeField] private int m_index;
    [SerializeField] private Image m_image;
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private TextMeshProUGUI m_description;

    public void Init(ClearReward clearReward)
    {
        // m_clearReward = clearReward;
        // //m_image.sprite = clearReward.m_sprite;
        // m_name.text = clearReward.m_name;
        // m_description.text = clearReward.m_description;
    }

    public void B_Click()
    {
        var player = PlayerManager.instance.GetComponent<PlayerManager>();
        switch ((RewardType)m_index)
        {
            case RewardType.Hp : player.m_healthPlus += 4; break;
            case RewardType.Power : player.m_powerPlus += 2; break;
            case RewardType.Speed : player.m_speedPlus += 2; break;
        }
        ;
        GameManager.Instance.SelectClearReward(m_clearReward);
    }
}
