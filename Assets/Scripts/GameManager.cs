using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : SingleTone<GameManager>
{
    #region PublicVariables

    public int m_currentStage;
    
    public MonsterType m_keywordMonsterType = MonsterType.melee;
    public RoomType m_keywordRoomType = RoomType.Gift;
    public bool m_keywordReword;
    
    public bool isGameOver = false;
    #endregion

    
    void Start()
    {
        Time.timeScale = 1f;
        GameStart();
    }

    #region PrivateVariables

    #endregion

    #region PublicMethod

    public void GameStart()
    {
        m_keywordMonsterType = (MonsterType)Random.Range(0, Enum.GetNames(typeof(MonsterType)).Length);
        UIManager.Instance.UpdateMonsterTypeText(m_keywordMonsterType);
        UIManager.Instance.UpdateRoomTypeText(m_keywordRoomType);
        UIManager.Instance.ShowKeywordPanel();
    
        m_currentStage++;
        
        List<RoomType> roomTypes= new() { RoomType.Gift, RoomType.NormalGift } ;
        m_keywordRoomType = roomTypes[Random.Range(0, roomTypes.Count)];
        
        //TODO : A 입력 시 패널 끄기, 패널 켜져있는 동안 다른 입력 못받도록
        UIManager.Instance.HideKeywordPanel();
    }
    
    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOverPanel();
        Time.timeScale = 0f;
    }
    public void GameClear()
    {
        m_currentStage++;
        
        isGameOver = true;
        UIManager.Instance.ShowClearPanel();
        Time.timeScale = 0f;
    }
    
    
    #endregion

    #region PrivateMethod

    #endregion
}