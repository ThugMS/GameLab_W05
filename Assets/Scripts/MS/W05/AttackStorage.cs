using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStorage : MonoBehaviour
{
    #region PublicVariables
    public bool m_isElectric = false;
    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    private void Update()
    {
        if(m_isElectric == true)
        {
            Tear[] objs = gameObject.GetComponentsInChildren<Tear>();

            for(int i=0;i<objs.Length-1; i++)
            {
                objs[i].SetElectric(objs[i+1].gameObject);
            }
        }
        
    }
    #endregion

    #region PrivateMethod

    #endregion
}
