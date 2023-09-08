using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wizard : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Attack")]
    [SerializeField] protected GameObject m_attackPrefab;
    [SerializeField] protected float m_attackMaxSpeed;
    #endregion

    #region PublicMethod
    protected override void SetStatus()
    {

    }

    protected override void Attack()
    {
        CreateAttack();
    }

    protected override void Ability()
    {

    }
    #endregion

    #region PrivateMethod
    private void CreateAttack()
    {
        GameObject obj = Instantiate(m_attackPrefab, transform.position, Quaternion.identity, transform);

    }
    #endregion
}
