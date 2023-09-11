using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
 
public class Archer : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] protected int m_attackLayerMask;

    [Header("Arrow")]
    [SerializeField] protected GameObject m_arrow;

    [Header("Attack")]
    [SerializeField] protected bool m_isReadyArrow = false;
    [SerializeField] protected bool m_canArrow = false;
    [SerializeField] protected float m_arrowInitSpeed = 5f;
    [SerializeField] protected float m_arrowCurSpeed = 5f;
    [SerializeField] protected float m_arrowMaxSpeed = 20f;
    [SerializeField] protected float m_arrowAddSpeed = 0.1f;
    [SerializeField] protected float m_minReadyTime = 0.5f;
    [SerializeField] protected float m_arrowInitPower = 2f;
    [SerializeField] protected float m_arrowCurPower = 2f;
    [SerializeField] protected float m_arrowAddPower = 0.1f;
    [SerializeField] protected float m_arrowMaxPower = 5f;

    [Header("Ability")]
    [SerializeField] protected float m_backStepDis = 3f;
    [SerializeField] protected float m_durationTime = 0.2f;
    [SerializeField] protected int m_backStepLayerMask;
    [SerializeField] Ease m_backStepEase = Ease.Linear;
    [SerializeField] protected Collider2D[] m_colliders;
    [SerializeField] protected Vector2 m_backStopBoxSize;
    [SerializeField] protected Vector2 m_backStepDir;
    [SerializeField] private bool m_canAbility = true;

    [Header("Effect")]
    [SerializeField] private GameObject m_dashEffect;
    [SerializeField] private GameObject m_abilityEffect;
    [SerializeField] private GameObject m_abilityDamageEffect;
    #endregion

    #region Test
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Vector2 attackDir = m_Direction.normalized * (m_offset + m_backStopBoxSize.x / 2);
    //    Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

    //    float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

    //    Gizmos.DrawWireCube(attackPos, m_backStopBoxSize);
    //}
    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();
        
        m_backStepLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
        m_attackLayerMask = LayerMask.GetMask("Monster", "Boss");
        
        SetPlayerClassType(PlayerClassType.Archer);
    }

    public override void OnAttack(InputAction.CallbackContext _context)
    {
        if (_context.started == true)
        {
            m_isReadyArrow = true;
            m_canMove = false;
            ReadyArrow();
            StartAttackState();
        }

        if(_context.canceled == true)
        {
            m_canMove = true;
            EndAttack();
            ShootArrow();
            ResetArrowStat();
        }
    }
    protected override void SetStatus()
    {
        OnStatusChanged();
    }
    

    protected override void Attack()
    {   
        ReadyArrow();
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

        m_canAct = false;
        m_canMove = false;
        m_canAbility = false;
        m_backStepDir = m_Direction;

        BackStep();
        UIManager.Instance.GetSkillCoolTime(m_coolTime);
        StartCoroutine(nameof(IE_DashCoolTime));
    }

    public void EndAttack()
    {
        m_animator.SetBool("IsAttack", false);
        SetCanMove(true);
        SetCanAct(true);
        m_isAct = false;
        m_isReadyArrow = false;

        if (m_inputDirection != Vector2.zero)
        {
            m_isMove = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(m_isReadyArrow == true)
        {
            m_animator.SetFloat("XDir", m_Direction.x);
            m_animator.SetFloat("YDir", m_Direction.y);
        }
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
    
    private void ReadyArrow()
    {
        StartCoroutine(nameof(IE_ReadyArrowTime));
    }

    private void ShootArrow()
    {
        StopCoroutine(nameof(IE_ReadyArrowTime));

        if (m_canArrow == false)
        {   
            return;
        }
            
        
        EndAttack();

        float angle = Vector2.SignedAngle(Vector2.up, m_Direction.normalized);

        GameObject arrow = Instantiate(m_arrow, transform.position, Quaternion.Euler(0, 0, angle));
        arrow.GetComponent<Arrow>().InitSetting(m_arrowCurSpeed, m_Direction.normalized, m_power);
    }

    private void ResetArrowStat()
    {
        m_arrowCurSpeed = m_arrowInitSpeed;
        m_arrowCurPower = m_arrowInitPower;
        m_canArrow = false;
    }

    private void SpawnEffect()
    {
        GameObject obj = Instantiate(m_dashEffect, transform.position, Quaternion.identity);
        Animator animator = obj.GetComponent<Animator>();

        float angle = Vector2.Angle(Vector2.right, m_backStepDir.normalized);
        m_abilityEffect.SetActive(true);
        m_abilityEffect.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 pos = m_Direction.normalized * m_offset * 3;
        m_abilityEffect.transform.position = transform.position + pos;

        animator.SetFloat("Xdir", m_Direction.x);
        animator.SetFloat("Ydir", m_Direction.y);

    }
    private void BackStep()
    {
        SpawnEffect();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, -m_backStepDir, m_backStepDis, m_backStepLayerMask);
        Tweener tween = null;

        if (hit == true)
        {
            tween = transform.DOMove(hit.point, m_durationTime).SetEase(m_backStepEase);
            StartCoroutine(nameof(IE_BackStepAttack), tween);
        }
        else
        {
            Vector3 dis = -m_backStepDir.normalized * m_backStepDis;
            Vector3 targetPos = transform.position + dis;

            tween = transform.DOMove(targetPos, m_durationTime).SetEase(m_backStepEase);
            StartCoroutine(nameof(IE_BackStepAttack), tween);
        }
    }

    private void AttackCheckCollider()
    {
        m_colliders = null;

        Vector2 attackDir = m_backStepDir.normalized * (m_offset + m_backStopBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_backStepDir.normalized);

        m_colliders = Physics2D.OverlapBoxAll(attackPos, m_backStopBoxSize, angle, m_attackLayerMask);
    }

    public void DamageAttackMonster()
    {
        int cnt = 0;

        foreach (var iter in m_colliders)
        {
            BaseMonster monster;

            iter.TryGetComponent<BaseMonster>(out monster);

            monster.getDamage(m_power * 2);

            cnt++;

            Instantiate(m_abilityDamageEffect, monster.transform.position, Quaternion.identity);

            if (cnt >= 3)
            {
                break;
            }
        }
    }

    private IEnumerator IE_ReadyArrowTime()
    {
        yield return new WaitForSeconds(m_minReadyTime);
        m_canArrow = true;

        while (true)
        {
            m_arrowCurSpeed += m_arrowAddSpeed;
            m_arrowCurPower += m_arrowAddPower;

            if(m_arrowCurPower >= m_arrowMaxPower)
            {
                m_arrowCurPower = m_arrowMaxPower;
            }

            if(m_arrowCurSpeed >= m_arrowMaxSpeed)
            {
                m_arrowCurSpeed = m_arrowMaxSpeed;
            }

            yield return null;
        }
    }

    private IEnumerator IE_BackStepAttack(Tweener _tween)
    {
        yield return _tween.WaitForCompletion();

        EndAttack();
        AttackCheckCollider();
    }

    private IEnumerator IE_DashCoolTime()
    {
        yield return new WaitForSeconds(m_coolTime);

        m_canAbility = true;
    }
    #endregion
}
