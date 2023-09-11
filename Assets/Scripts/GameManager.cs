using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    #region PublicVariables
    public int m_currentStage;
    
    public MonsterType m_keywordMonsterType = MonsterType.melee;
    public RoomType m_keywordRoomType = RoomType.Gift;
    public bool m_keywordReword;

    private List<MonsterType> _appearMonsterTypes = new()
    {
        MonsterType.ranged,
        MonsterType.melee,
        MonsterType.hover,
    };
    
    private List<RoomType> _appearRoomTypes = new()
    {
        RoomType.Gift,
        RoomType.NormalGift,
        RoomType.Normal,
    };
    
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

        // 랜덤 타입 설정 및 제거
        m_keywordMonsterType = _appearMonsterTypes[ (int)Random.Range(0, Time.deltaTime) % _appearMonsterTypes.Count];
        _appearMonsterTypes.Remove(m_keywordMonsterType);
        m_keywordRoomType = _appearRoomTypes[(int)Random.Range(0, Time.deltaTime) % _appearRoomTypes.Count];
        _appearRoomTypes.Remove(m_keywordRoomType);
        
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