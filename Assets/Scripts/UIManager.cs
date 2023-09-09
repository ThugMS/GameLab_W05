using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    #region PublicVariables
    
    [Header("Panel")]
    [SerializeField] private List<GameObject> m_panel;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_clearPanel;
    [SerializeField] private GameObject m_hitPanel; 
    
    
    [Header("Heart")]
    [SerializeField] private Transform m_heartPanel;
    [SerializeField] private GameObject m_heartPrefab;
    [SerializeField] private List<Heart> hearts = new List<Heart>();
    
    public int maxHeart = 5;
    public int currentActiveHeart = 3;

    #endregion

    #region PrivateVariables
    #endregion
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Update()
    {
        CheckMonster();
    }
    
    #region PublicMethod
    
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
    
    #region Heart
    public void SetHeartUI()
    {
        ClearHeartUI();
        CreateHeartUI();
    }
    public void DecreaseHeart(int decreaseAmount)
    {
        StartCoroutine(IE_HitEffect());

        HeartStatus decreaseBy = (HeartStatus)decreaseAmount;
        Heart lastActiveHeart = hearts.FindLast(heart => heart.GetHeartStatus() != HeartStatus.Empty);

        HeartStatus newStatus = lastActiveHeart.GetHeartStatus() - (int)decreaseBy;
        lastActiveHeart.SetHeartImage(newStatus < HeartStatus.Empty ? HeartStatus.Empty : newStatus);

        if (hearts.All(heart => heart.GetHeartStatus() == HeartStatus.Empty))
        {
            StartCoroutine(IE_GameOverEffect());
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
    
    private void CreateHeartUI()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            GameObject newHeart = Instantiate(m_heartPrefab, m_heartPanel);
            Heart heart = newHeart.GetComponent<Heart>();
            hearts.Add(heart);
    
            heart.SetHeartImage(i < currentActiveHeart ? HeartStatus.Full : HeartStatus.Empty);
            newHeart.SetActive(true);
        }
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