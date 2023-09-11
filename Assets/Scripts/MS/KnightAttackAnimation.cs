using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackAnimation : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Knight m_knight;
    #endregion

    #region PublicMethod
    public void Attack()
    {
        m_knight.DamageAttackMonster();
    }

    public void Ability()
    {
        m_knight.DamageAttackMonster();
    }

    public void Dead()
    {
        UIManager.Instance.PlayGameOverEffect();
    }

    public void EndAttack()
    {
        m_knight.SetCanMove(true);
        m_knight.SetCanAct(true);
        m_knight.EndAttackAnimation();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
