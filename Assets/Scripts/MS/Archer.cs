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
    [SerializeField] protected float m_offset = 0.5f;
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
    #endregion

    #region Test
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 attackDir = m_Direction.normalized * (m_offset + m_backStopBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

        Gizmos.DrawWireCube(attackPos, m_backStopBoxSize);
    }
    #endregion

    #region PublicMethod
    protected override void Start()
    {
        base.Start();
        
        m_backStepLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
        m_attackLayerMask = LayerMask.GetMask("Monster", "Boss");
    }

    public override void OnAttack(InputAction.CallbackContext _context)
    {
        if (_context.started == true)
        {
            m_isReadyArrow = true;
            m_canMove = false;
            ReadyArrow();
        }

        if(_context.canceled == true)
        {
            m_canMove = true;
            ShootArrow();
            ResetArrowStat();
        }
    }
    protected override void SetStatus()
    {
        m_power = 3;
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

        m_canAct = false;
        m_canMove = false;

        BackStep();
    }

    public void EndAttack()
    {
        SetCanMove(true);
        SetCanAct(true);
    }
    #endregion

    #region PrivateMethod
    private void ReadyArrow()
    {
        StartCoroutine(nameof(IE_ReadyArrowTime));
    }

    private void ShootArrow()
    {   
        if(m_canArrow == false)
        {
            return;
        }
            
        StopCoroutine(nameof(IE_ReadyArrowTime));

        float angle = Vector2.SignedAngle(Vector2.up, m_Direction.normalized);
        Debug.Log(angle);

        GameObject arrow = Instantiate(m_arrow, transform.position, Quaternion.Euler(0, 0, angle), transform);
        arrow.GetComponent<Arrow>().InitSetting(m_arrowCurSpeed, m_Direction.normalized);
    }

    private void ResetArrowStat()
    {
        m_arrowCurSpeed = m_arrowInitSpeed;
        m_arrowCurPower = m_arrowInitPower;
        m_canArrow = false;
    }

    private void BackStep()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, -m_Direction, m_backStepDis, m_backStepLayerMask);
        Tweener tween = null;

        if (hit == true)
        {
            tween = transform.DOMove(hit.point, m_durationTime).SetEase(m_backStepEase);
            StartCoroutine(nameof(IE_BackStepAttack), tween);
        }
        else
        {
            Vector3 dis = -m_Direction.normalized * m_backStepDis;
            Vector3 targetPos = transform.position + dis;

            tween = transform.DOMove(targetPos, m_durationTime).SetEase(m_backStepEase);
            StartCoroutine(nameof(IE_BackStepAttack), tween);
        }
    }

    private void AttackCheckCollider()
    {
        m_colliders = null;

        Vector2 attackDir = m_Direction.normalized * (m_offset + m_backStopBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

        m_colliders = Physics2D.OverlapBoxAll(attackPos, m_backStopBoxSize, angle, m_attackLayerMask);

        DamageAttackMonster();
    }

    public void DamageAttackMonster()
    {
        int cnt = 0;

        foreach (var iter in m_colliders)
        {
            BaseMonster monster;
            Debug.Log("1");
            iter.TryGetComponent<BaseMonster>(out monster);

            monster.getDamage(m_power);
            cnt++;

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
    #endregion

}
