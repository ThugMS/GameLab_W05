using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationScript : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    public void Dead()
    {
        UIManager.Instance.PlayGameOverEffect();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
