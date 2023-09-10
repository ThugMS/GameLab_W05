using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBossAnimation : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private HoverBoss m_boss;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_boss = transform.GetComponentInParent<HoverBoss>();
    }

    public void Attack()
    {
        m_boss.AttackPlayer();
    }

    public void EndAttack()
    {
        m_boss.EndAttack();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
