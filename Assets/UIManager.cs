using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    #region PublicMethod
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
}