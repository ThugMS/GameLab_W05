using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
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
    [SerializeField] private Image m_abilityCoolTime;
    
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
    [SerializeField] private List<GemButton> m_gemPanelButtons = new List<GemButton>();
    [SerializeField] private int m_gemListIndex = 0;
    
    [Header("KeywordPanel")]
    [SerializeField] private GameObject m_keywordPanel;
    [SerializeField] private TextMeshProUGUI m_monsterTypeText;
    [SerializeField] private TextMeshProUGUI m_roomTypeText;
    [SerializeField] private TextMeshProUGUI m_rewardTypeText;

    
    
    #endregion

    #region PrivateVariables
    #endregion

    private void Update()
    {
        CheckMonster();
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
    
    public void ReLoadScene()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    #endregion
    
    #region InputSysewm
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

    public void GetGemPanelInput(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {   
            
            m_gemListIndex++;
            if (m_gemListIndex >= m_gemPanelButtons.Count)
            {
            }
        }
    }

    #endregion
    public void ShowKeywordPanel()
    {
        m_keywordPanel.SetActive(true);
    }
    public void HideKeywordPanel()
    {
        m_keywordPanel.SetActive(false);
        PlayerManager.instance.m_player.SetActive(true);
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

    public void UpdateRoomTypeText(RoomType _roomType)
    {
        switch (_roomType)
        {
            case RoomType.Normal:
                m_roomTypeText.text = "고난";
                break;
            case RoomType.Gift:
                m_roomTypeText.text = "축복";
                break;
            case RoomType.NormalGift:
                m_roomTypeText.text = "쟁취";
                break;
            default:
                m_roomTypeText.text = "Unknown";
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
        
        m_gemPanelButtons.Add(m_gemButtonCurrent);
        m_gemPanelButtons.Add(m_gemButtonGetted);
    }

    public void CloseGemPanel()
    {
        m_gemPanel.SetActive(false);
        m_gemPanelButtons = null;
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
    public void GetSkillCoolTime(float _time)
    {
        m_abilityCoolTime.fillAmount = 1f;
        m_abilityCoolTime.DOFillAmount(0f, _time);
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
    public void IncreaseHeart(float currentHP, float maxHP)
    {
        //TODO : 뭔가 이펙트
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