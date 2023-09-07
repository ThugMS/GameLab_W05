using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region PublicVariables
    public bool isGameOver = false;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        Time.timeScale = 1f;
        UIManager.Instance.SetLifeUI();
    }

    #region PrivateVariables

    #endregion

    #region PublicMethod
    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOverPanel();
        Time.timeScale = 0f;
    }
    public void GameClear()
    {
        isGameOver = true;
        UIManager.Instance.ShowClearPanel();
        Time.timeScale = 0f;
    }
    
    
    #endregion

    #region PrivateMethod

    #endregion
}