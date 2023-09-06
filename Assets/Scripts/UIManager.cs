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
    
    
    [Header("Life")]
    [SerializeField] private Transform m_lifePanel;
    [SerializeField] private GameObject m_lifePrefab;
    
    public int maxLife = 3;
    public int currentActiveLife = 3;
    public int currentActiveIcon = 3;
    public float decreaseAmount = 0.5f;
    
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
    
    #region life
     public void SetLifeUI()
    {
        foreach (Transform child in m_lifePanel)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < maxLife; i++)
        {
            GameObject newLife = Instantiate(m_lifePrefab, m_lifePanel);
            newLife.SetActive(i < currentActiveLife);
            
            //Bg가 계속 사라져서 임시
            Transform lifeBgTransform = newLife.transform.Find("LifeBg");
            Image lifeBgImage = lifeBgTransform?.GetComponent<Image>();
            lifeBgImage.enabled = true;
        }
    }
     public void DecreaseLife()
     {
         StartCoroutine(IE_HitEffect());
         Transform lastActiveLife = m_lifePanel.Cast<Transform>()
             .LastOrDefault(life => life.GetComponentsInChildren<Image>()
                 .Any(img => img.gameObject.name == "LifeIcon" && img.gameObject.activeSelf));

         if (lastActiveLife == null) return;

         Image lifeImage = lastActiveLife.GetComponentsInChildren<Image>()
             .FirstOrDefault(img => img.gameObject.name == "LifeIcon");

         if (lifeImage == null) return;

         lifeImage.fillAmount -= decreaseAmount;

         if (lifeImage.fillAmount > 0) return;

         lifeImage.gameObject.SetActive(false);

         if (--currentActiveIcon > 0) return;

         StartCoroutine(IE_GameOverEffect());
     }
    
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
    
    #region PrivateMethod
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
        
        if (!isAnyMonsterInView)
        {
            GameManager.Instance.GameClear();
        }
    }
    #endregion
}