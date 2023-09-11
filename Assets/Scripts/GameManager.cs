using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    #region PublicVariables
    public int m_currentStage;
    
    public MonsterType m_keywordMonsterType = MonsterType.melee;
    public RoomType m_keywordRoomType = RoomType.Gift;
    public bool m_keywordReword;
    
    public bool isGameOver = false;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_currentStage = 1;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameStart();
    }

    #region PublicMethod

    public void GameStart()
    {

        PlayerManager.instance.SetPlayer(GameObject.FindWithTag("Player"));;
        PlayerManager.instance.GetPlayer().SetActive(false);

        
        m_keywordMonsterType = (MonsterType)Random.Range(0, Enum.GetNames(typeof(MonsterType)).Length);
        List<RoomType> roomTypes = new() { RoomType.Gift, RoomType.NormalGift } ;
        m_keywordRoomType = roomTypes[Random.Range(0, roomTypes.Count)];
        UIManager.Instance.UpdateMonsterTypeText(m_keywordMonsterType);
        UIManager.Instance.UpdateRoomTypeText(m_keywordRoomType);
        UIManager.Instance.ShowKeywordPanel();
    }

    public void PressAOnKeywordPanel()
    {
        UIManager.Instance.HideKeywordPanel();
        PlayerManager.instance.GetPlayer().SetActive(true);
    }
    
    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOverPanel();
        Time.timeScale = 0f;
    }
    public void GameClear()
    {
        PlayerManager.instance.GetPlayer().SetActive(false);
        
        if (m_currentStage < 3)
        {
            UIManager.Instance.ShowRewardPanel();
        }
        else
        {
            isGameOver = true;
            UIManager.Instance.ShowClearPanel();
            Time.timeScale = 0f;
        }
    }

    void NextStage()
    {
        m_currentStage++;
        PlayerManager.instance.SavePlusStat();
        SceneManager.LoadScene("Ingame");
        Invoke(nameof(GameStart), .005f);
    }

    public void SelectClearReward(ClearReward reward)
    {
        // [TODO] Reward 획득 
        NextStage();   
    }
    
    #endregion
}