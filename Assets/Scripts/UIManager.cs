using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    #region PublicVariables
    public List<GameObject> panel;
    #endregion

    #region PrivateVariables
    #endregion
    
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
    public void ClosePanel()
    {
        for (int i = panel.Count - 1; i >= 0; i--)
        {
            if (panel[i].activeSelf)
            {
                panel[i].SetActive(false);
                break;
            }
        }
    }
    #endregion
    #endregion
    
    #region PrivateMethod
    #endregion
}