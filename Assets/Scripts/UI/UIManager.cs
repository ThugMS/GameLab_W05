using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [SerializeField] private List<GameObject> m_panel;
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
    
    #region Panel
    public void ShowGameOverPanel()
    {
        m_gameOverPanel.SetActive(true);
    }
    public void ShowClearPanel()
    {
        m_clearPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        for (int i = m_panel.Count - 1; i >= 0; i--)
        {
            if (m_panel[i].activeSelf)
            {
                m_panel[i].SetActive(false);
                break;
            }
        }
    }
    #endregion

    #region SkillSlot

    public void SetSKillSlot(Player.PlayerClassType playerClassType)
    {
        m_skillPaenl.SetActive(true);
        
        var attackIcon = ResourceManager.Instance.GetSkillSlotAttackIcon(playerClassType);
        var abilityIcon = ResourceManager.Instance.GetSkillSlotAbilityIcon(playerClassType);
        
        m_attackSlot.sprite = attackIcon;
        m_abilitySlot.sprite = abilityIcon;
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
        int maxHeart = (int)System.Math.Truncate(maxHP / 4);
        
        for (int i = 1; i <= maxHeart; i++)
        {
            Heart heart = GetOrCreateHeart(i - 1);
            HeartStatus status = DetermineHeartStatus(currentHP, i);
            heart.SetHeartImage(status);
            heart.gameObject.SetActive(true);
        }
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