using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public static PlayerManager instance;

    #region PublicVariables
    public GameObject m_player;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetPlayer()
    {
        return m_player;
    }

    public void SetPlayer(GameObject _obj)
    {
        m_player = _obj;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
