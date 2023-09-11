using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    #region PublicVariables
    #endregion
    public void LoadIngameScene()
    {
        SceneManager.LoadScene("Ingame");
        GameManager.Instance.GameStart();
    }
    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
}