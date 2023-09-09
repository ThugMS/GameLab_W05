using DG.Tweening;
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

    [Header("Ability")]
    [SerializeField] protected GameObject m_abilityPrefab;
    [SerializeField] protected float m_portDis = 3f;
    [SerializeField] protected Vector2 m_portDir;
    [SerializeField] protected int m_portLayerMask;
    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();

        m_portLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
    }
    protected override void SetStatus()
    {

    }

    protected override void Attack()
    {
        CreateAttack();
        StartAttackState();
    }

    protected override void Ability()
    {
        m_portDir = m_Direction;
        Port();
    }
    #endregion

    #region PrivateMethod
    private void StartAttackState()
    {
        m_animator.SetBool("IsAttack", true);
        SetCanMove(false);
        SetCanAct(false);

        m_isMove = false;
        m_isAct = true;
    }

    private void CreateAttack()
    {
        Vector3 offsetPositon = (m_offset + m_attackRadius) * m_Direction.normalized;
        GameObject obj = Instantiate(m_attackPrefab, transform.position + offsetPositon, Quaternion.identity, transform);
        obj.GetComponent<WizardAttack>().InitSetting(m_Direction);
    }

    private void Port()
    {
        CreateBomb();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, m_portDir, m_portDis + m_offset, m_portLayerMask);

        if (hit == true)
        {
            Vector2 offsetPos = m_portDir.normalized * m_offset;
            Vector3 pos = hit.point + offsetPos;
            transform.position = pos;
        }
        else
        {
            Vector3 dis = m_portDir.normalized * m_portDis;
            Vector3 targetPos = transform.position + dis;

            transform.position = targetPos;
        }
    }

    private void CreateBomb()
    {
        GameObject obj = Instantiate(m_abilityPrefab, transform.position, Quaternion.identity);
    }
    #endregion
}
