using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region PublicVariables

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod

    #endregion

    #region PrivateMethod

    #endregion
}