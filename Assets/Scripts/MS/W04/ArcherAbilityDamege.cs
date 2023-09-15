using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAbilityDamege : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Archer m_parent;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_parent = GetComponentInParent<Archer>();
    }

    public void AbilityToMonster()
    {
        m_parent.DamageAttackMonster();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
