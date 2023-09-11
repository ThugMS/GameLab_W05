using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimationScript : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Wizard m_wizard;
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
