using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : SingleTone<UIManager>
{
    #region PublicVariables
    [Header("Keybinding Images")]
    [SerializeField] private KeyHint[] m_keyHints;

    private PlayerInput m_playerInput;
    
    [Header("Panel")]
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_clearPanel;
    [SerializeField] private GameObject m_hitPanel; 
    
    
    [Header("Heart")]
    [SerializeField] private Transform m_heartPanel;
    [SerializeField] private GameObject m_heartPrefab;
    [SerializeField] private List<Heart> hearts = new List<Heart>();

    [Header("SkillSlot")]
    [SerializeField] private GameObject m_skillPaenl;
    [SerializeField] private Image m_attackSlot;
    [SerializeField] private Image m_abilitySlot;
    
    [Header("Profile")]
    [SerializeField] private Image m_profileImage;
    [SerializeField] private TextMeshProUGUI m_heartText;
    [SerializeField] private TextMeshProUGUI m_powerText;
    [SerializeField] private TextMeshProUGUI m_speedText;
    
    [SerializeField] private TextMeshProUGUI m_plusHeartText;
    [SerializeField] private TextMeshProUGUI m_plusPowerText;
    [SerializeField] private TextMeshProUGUI m_plusSpeedText;

    [SerializeField] private Image m_gemImage;

    [Header("GemPanel")]
    [SerializeField] private GameObject m_gemPanel;
    [SerializeField] private GemButton m_gemButtonCurrent;
    [SerializeField] private GemButton m_gemButtonGetted;
    
    [Header("KeywordPanel")]
    [SerializeField] private GameObject m_keywordPanel;
    [SerializeField] private TextMeshProUGUI m_monsterTypeText;
    #endregion

    #region PrivateVariables
    #endregion
    

    private void Update()
    {
        CheckMonster();
    }

    public void SetPlayerInput(PlayerInput input)
    {
        FieldInfo fieldInfo = typeof(PlayerInput).GetField("m_ControlsChangedEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            PlayerInput.ControlsChangedEvent controlsChangedAction = (PlayerInput.ControlsChangedEvent)fieldInfo.GetValue(input);

            controlsChangedAction.RemoveListener(OnControlsChanged);
            controlsChangedAction.AddListener(OnControlsChanged);

            fieldInfo.SetValue(input, controlsChangedAction);
        }
    }
    
    
    #region PublicMethod
    #region KeyHint
    public void OnControlsChanged(PlayerInput input)
    {
        foreach (var keyHint in m_keyHints)
        {
            keyHint.OnControlsChanged(input);
        }
    }
    
    #endregion
    
    #region Scene
    public void LoadIngameScene()
    {
        SceneManager.LoadScene("Ingame");
        GameManager.Instance.GameStart();
    }
    
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    
    public void ReLoadScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    #endregion
    
    public void ShowKeywordPanel()
    {
        m_keywordPanel.SetActive(true);
    }
    public void HideKeywordPanel()
    {
        m_keywordPanel.SetActive(false);
    }
    public void UpdateMonsterTypeText(MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.melee:
                m_monsterTypeText.text = "근거리";
                break;
            case MonsterType.ranged:
                m_monsterTypeText.text = "원거리";
                break;
            case MonsterType.hover:
                m_monsterTypeText.text = "공중";
                break;
            default:
                m_monsterTypeText.text = "Unknown";
                break;
        }
    }
    
    #region Panel
    public void ShowGameOverPanel()
    {
        m_gameOverPanel.SetActive(true);
    }
    public void ShowClearPanel()
    {
        m_clearPanel.SetActive(true);
    }
    #endregion

    #region Gem

    public void OpenGemPanel(Player player, Gem gem)
    {
        Time.timeScale = 0f;
        m_gemPanel.SetActive(true);
        
        m_gemButtonCurrent.Init(player, player.CurrentGemType);
        m_gemButtonGetted.Init(player, gem.GemType);
    }

    public void CloseGemPanel()
    {
        m_gemPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnGemChanged(Player player)
    {
        if (player.CurrentGemType == GemType.None)
        {
            m_gemImage.gameObject.SetActive(false);
        }
        else
        {
            m_gemImage.sprite = ResourceManager.Instance.GetGemIconImage(player.CurrentGemType);
            m_gemImage.gameObject.SetActive(true);
        }
    }

    #endregion
    
    #region SkillSlot

    public void SetSKillSlot(PlayerClassType playerClassType)
    {
        m_skillPaenl.SetActive(true);
        
        var attackIcon = ResourceManager.Instance.GetSkillSlotAttackIcon(playerClassType);
        var abilityIcon = ResourceManager.Instance.GetSkillSlotAbilityIcon(playerClassType);
        
        m_attackSlot.sprite = attackIcon;
        m_abilitySlot.sprite = abilityIcon;
    }

    #endregion

    #region Status

    public void SetProfileStatus(float baseHP, float basePower, float baseSpeed, float plusHP, float plusPower, float plusSpeed)
    {
        m_heartText.text = CalcHeartNum(baseHP).ToString();
        m_powerText.text = String.Format("{0:0.#}", basePower);
        m_speedText.text = String.Format("{0:0.#}", baseSpeed);
        
        m_plusHeartText.text = "+" + CalcHeartNum(plusHP).ToString();
        m_plusPowerText.text = String.Format("+{0:0.#}", plusPower);
        m_plusSpeedText.text = String.Format("+{0:0.#}", plusSpeed);
    }

    public void SetProfileImage(PlayerClassType playerClassType)
    {
        var profileImage = ResourceManager.Instance.GetPlayerClassProfileIcon(playerClassType);
        m_profileImage.sprite = profileImage;
    }

    #endregion
    
    #region Heart
    public void SetHeartUI(float currentHP, float maxHP)
    {
        ClearHeartUI();
        SetCurrentHpUI(currentHP, maxHP);
    }
    public void DecreaseHeart(float currentHP, float maxHP)
    {
        StartCoroutine(IE_HitEffect());
        SetCurrentHpUI(currentHP, maxHP);
    }

    public void PlayGameOverEffect()
    {
        StartCoroutine(IE_GameOverEffect());
    }

    private void SetCurrentHpUI(float currentHP, float maxHP)
    {
        int maxHeart = CalcHeartNum(maxHP);
        
        for (int i = 1; i <= maxHeart; i++)
        {
            Heart heart = GetOrCreateHeart(i - 1);
            HeartStatus status = DetermineHeartStatus(currentHP, i);
            heart.SetHeartImage(status);
            heart.gameObject.SetActive(true);
        }
    }

    private int CalcHeartNum(float hp)
    {
        return (int)System.Math.Truncate(hp / 4);
    }

    private Heart GetOrCreateHeart(int idx)
    {
        if (hearts.Count > idx)
        {
            return hearts[idx];
        }
        GameObject newHeart = Instantiate(m_heartPrefab, m_heartPanel);
        Heart heart = newHeart.GetComponent<Heart>();
        hearts.Add(heart);
        return heart;
    }
    
    private HeartStatus DetermineHeartStatus(float currentHP, int heartIndex)
    {
        int heartHealth = heartIndex * 4;

        if (currentHP >= heartHealth)
        {
            return HeartStatus.Full;
        }
        else
        {
            int remain = heartHealth - (int)currentHP;
            return remain < 4 ? (HeartStatus)(4 - remain) : HeartStatus.Empty;
        }
    }

    #endregion
    
    #endregion
    
    #region PrivateMethod
    
    #region SetHeartUI
    private void ClearHeartUI()
    {
        foreach (Transform child in m_heartPanel)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();
    }
    
    #endregion
    
    private void CheckMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        
        bool isAnyMonsterInView = false;
        Camera cam = Camera.main;

        foreach (GameObject monster in monsters)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(monster.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
                isAnyMonsterInView = true;
                break;
            }
        }

        // if (!isAnyMonsterInView)
        // {
        //     GameManager.Instance.GameClear();
        // }
    }
    
    #region IE_Effect
    private IEnumerator IE_GameOverEffect()
    {
        m_hitPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_hitPanel.SetActive(false);
        GameManager.Instance.GameOver();
    }
    private IEnumerator IE_HitEffect()
    {
        for (int i = 0; i < 1; i++)
        {
            m_hitPanel.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            m_hitPanel.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
    }
    #endregion
    #endregion
}