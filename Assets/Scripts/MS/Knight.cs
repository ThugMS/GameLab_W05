using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knight : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private Collider2D[] m_colliders = null;

    [Header("Attack")]
    [SerializeField] private Vector2 m_attackBoxSize;
    [SerializeField] private float m_offset = 0.5f;
    [SerializeField] private float m_range = 2f;
    [SerializeField] private int m_attackLayerMask;

    [Header("Ability")]
    [SerializeField] private Vector2 m_dashAttackBoxSize;
    [SerializeField] private float m_dashDis;
    [SerializeField] private float m_durationTime = 0.5f;
    [SerializeField] private float m_coolTime = 3f;
    [SerializeField] private Ease m_dashEase = Ease.Linear;
    [SerializeField] private int m_dashLayerMask;

    [Header("Animator")]
    [SerializeField] private Animator m_animator;
    #endregion

    #region Test
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    #region Attack

    //    //Vector2 attackDir = m_Direction.normalized * (m_offset + m_boxSize.x / 2);
    //    //Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

    //    //Gizmos.DrawWireCube(attackPos, m_boxSize);
    //    #endregion

    //    #region Dash
    //    //RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2f, m_Direction, m_dashDis, m_dashLayerMask);

    //    //if (hit)
    //    //{
    //    //    Debug.Log(hit.point);
    //    //    Gizmos.DrawRay(transform.position, m_Direction * hit.distance);
    //    //}
    //    //else
    //    //{
    //    //    Gizmos.DrawRay(transform.position, m_Direction*m_dashDis);
    //    //}

    //    //Vector2 attackDir = m_Direction.normalized * (m_dashAttackBoxSize.x / 2);
    //    //Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

    //    //float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

    //    //Gizmos.DrawWireCube(attackPos, m_dashAttackBoxSize);
    //    #endregion
    //}
    #endregion

    #region PublicMethod
    protected override void SetStatus()
    {
        m_power = 5f;
    }

    protected override void Attack()
    {   
        if(m_canAct == false)
        {
            return;
        }

        m_animator.SetTrigger("AttackRight");
        m_canAct = false;
        m_canMove = false;

        AttackCheckCollider();
    }

    protected override void Ability()
    {
        Dash();
    }

    protected override void Start()
    {
        base.Start();

        m_attackLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_dashLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
    }


    public void DamageAttackMonster()
    {
        foreach (var iter in m_colliders)
        {
            BaseMonster monster;

            iter.TryGetComponent<BaseMonster>(out monster);

            monster.getDamage(m_power);
        }
    }
    #endregion

    #region PrivateMethod
    private void AttackCheckCollider()
    {
        m_colliders = null;

        Vector2 attackDir = m_Direction.normalized * (m_offset + m_attackBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

        m_colliders = Physics2D.OverlapBoxAll(attackPos, m_attackBoxSize, angle, m_attackLayerMask);
    }

    private RaycastHit2D Dash()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, m_Direction, m_dashDis, m_dashLayerMask);
        Tweener tween = null;

        if (hit == true)
        {
            tween = transform.DOMove(hit.point, m_durationTime).SetEase(m_dashEase);

        }
        else
        {
            Vector3 dis = m_Direction.normalized * m_dashDis;
            Vector3 targetPos = transform.position + dis;

            tween = transform.DOMove(targetPos, m_durationTime).SetEase(m_dashEase);

        }
        StartCoroutine(nameof(IE_DashAttack), tween);


        return hit;
    }

    private void DashAttackCheckCollider()
    {
        m_colliders = null;

        Vector2 attackDir = m_Direction.normalized * (m_dashAttackBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

        m_colliders = Physics2D.OverlapBoxAll(attackPos, m_dashAttackBoxSize, angle, m_attackLayerMask);
    }

    private IEnumerator IE_DashAttack(Tweener _tween)
    {
        yield return _tween.WaitForCompletion();

        DashAttackCheckCollider();

        DamageAttackMonster();
    }
    #endregion
}
