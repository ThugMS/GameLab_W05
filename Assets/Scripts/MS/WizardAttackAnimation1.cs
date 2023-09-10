using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttackAnimation1 : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_parent;
    #endregion

    #region PublicMethod
    public void EndAbilityAnimation()
    {
        Destroy(m_parent);
    }
    #endregion

    #region PrivateMethod
    #endregion
}
