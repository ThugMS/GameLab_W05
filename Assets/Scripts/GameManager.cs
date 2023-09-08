using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    #region PublicVariables

    private int m_currentStage;
    
    public MonsterType m_keywordMonsterType;
    public RoomType m_keywordRoomType;
    public bool m_keywordReword;
    
    public bool isGameOver = false;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_currentStage = 1;
        }
    }
    
    void Start()
    {
        Time.timeScale = 1f;
        UIManager.Instance.SetHeartUI();
    }

    #region PrivateVariables

    #endregion

    #region PublicMethod

    public void GameStart()
    {
        m_currentStage++;
         
        
        // [TODO] 키워드에 따른 화면 출력
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