using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemButton : MonoBehaviour
{
    [SerializeField] private Image m_gemImage;
    [SerializeField] private TextMeshProUGUI m_explainText;
    
    private GemType m_gemType;
    private const string m_gemPath = "Gem/";
    private const string HP_Explain_Text = "HP 1 증가";
    private const string Power_Explain_Text = "공격력 1 증가";
    private const string Speed_Explain_Text = "이동속도 1 증가";

    private Player m_player;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Init(Player player, GemType gemType)
    {
        m_player = player;
        m_gemType = gemType;

        m_gemImage.sprite = ResourceManager.Instance.GetGemImage(m_gemType);

        switch (m_gemType)
        {
            case GemType.HP:
                m_explainText.text = HP_Explain_Text;
                break;
            case GemType.Power:
                m_explainText.text = Power_Explain_Text;
                break;
            case GemType.Speed:
                m_explainText.text = Speed_Explain_Text;
                break;
        }
    }

    public void OnClick()
    {
        m_player.ChangeGem(m_gemType);
        UIManager.Instance.CloseGemPanel();
    }
    
}
