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
    [SerializeField] protected float m_attackRadius = 0.25f;
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
        Vector3 offsetPositon = (m_offset + m_attackRadius) * m_Direction.normalized;
        GameObject obj = Instantiate(m_attackPrefab, transform.position + offsetPositon, Quaternion.identity, transform);
        obj.GetComponent<WizardAttack>().InitSetting(m_Direction);
    }
    #endregion
}
