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
    [SerializeField] protected GameObject m_portEffect;
    [SerializeField] private bool m_canAbility = true;
    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();

        m_portLayerMask = LayerMask.GetMask("Wall");
        SetPlayerClassType(PlayerClassType.Wizard);
    }
    protected override void SetStatus()
    {
        
        OnStatusChanged();
    }

    protected override void Attack()
    {
        if (m_canAct == false)
        {
            return;
        }

        CreateAttack();
        StartAttackState();
        StartCoroutine(nameof(IE_DashCoolTime));
    }

    protected override void Ability()
    {
        if (m_canAct == false)
        {
            return;
        }

        if (m_canAbility == false)
        {
            return;
        }

        m_portDir = m_Direction;
        m_canAbility = false;

        Port();
        UIManager.Instance.GetSkillCoolTime(m_coolTime);
    }

    public void EndAttackState()
    {
        SetCanMove(true);
        SetCanAct(true);
        m_isAct = false;

        if (m_inputDirection != Vector2.zero)
        {
            m_isMove = true;
        }
    }
    #endregion

    #region PrivateMethod
    private void StartAttackState()
    {
        m_animator.SetTrigger("Attack");
        SetCanMove(false);
        SetCanAct(false);

        m_isMove = false;
        m_isAct = true;
    }

    

    private void CreateAttack()
    {
        float angle = Vector2.SignedAngle(Vector2.right, m_Direction.normalized);

        Vector3 offsetPositon = (m_offset + m_attackRadius) * m_Direction.normalized;
        GameObject obj = Instantiate(m_attackPrefab, transform.position + offsetPositon, Quaternion.Euler(0, 0, angle));
        obj.GetComponent<WizardAttack>().InitSetting(m_Direction, m_power);
    }

    private void Port()
    {
        CreateBomb();
        m_portEffect.SetActive(true);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, m_portDir, m_portDis + m_offset, m_portLayerMask);

        if (hit == true)
        {
            Vector3 pos;
            if (hit.distance < m_offset)
            {
                pos = transform.position;
            }
            else
            {
                Vector2 offsetPos = m_portDir.normalized * m_offset;
                pos = hit.point + offsetPos;
            }

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
        obj.GetComponent<WizardAbility>().Init(m_power);
    }

    private IEnumerator IE_DashCoolTime()
    {
        yield return new WaitForSeconds(m_coolTime);

        m_canAbility = true;
    }
    #endregion
}
