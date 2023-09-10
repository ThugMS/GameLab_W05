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

    private int m_currentStage;
    
    public MonsterType m_keywordMonsterType = MonsterType.melee;
    public RoomType m_keywordRoomType = RoomType.Gift;
    public bool m_keywordReword;
    
    public bool isGameOver = false;
    #endregion

    
    void Start()
    {
        Time.timeScale = 1f;
    }

    #region PrivateVariables

    #endregion

    #region PublicMethod

    public void GameStart()
    {
        m_currentStage++;
         
        // [TODO] 키워드에 따른 화면 출력
        m_keywordMonsterType = (MonsterType)Random.Range(0, Enum.GetNames(typeof(MonsterType)).Length);
        List<RoomType> roomTypes = new() { RoomType.Gift, RoomType.NormalGift };
        m_keywordRoomType = roomTypes[Random.Range(0, roomTypes.Count)];
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