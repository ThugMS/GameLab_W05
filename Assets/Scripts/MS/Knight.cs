using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knight : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
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
    protected override void Attack()
    {
        Collider2D[] collider = AttackCheckCollider();
    }

    protected override void Ability()
    {
        Dash();
    }

    protected override void SetStatus()
    {
        m_power = 5f;
    }

    protected override void Start()
    {
        base.Start();

        m_attackLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_dashLayerMask = LayerMask.GetMask("Wall", "Monster", "Boss");
    }
    #endregion

    #region PrivateMethod
    private Collider2D[] AttackCheckCollider()
    {
        Collider2D[] collider = null;

        Vector2 attackDir = m_Direction.normalized * (m_offset + m_attackBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);
       
        collider = Physics2D.OverlapBoxAll(attackPos, m_attackBoxSize, angle, 1 << LayerMask.NameToLayer("Monster"), 1 << LayerMask.NameToLayer("Boss"));

        return collider;
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

    private Collider2D[] DashAttackCheckCollider()
    {
        Collider2D[] collider = null;

        Vector2 attackDir = m_Direction.normalized * (m_dashAttackBoxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);

        collider = Physics2D.OverlapBoxAll(attackPos, m_dashAttackBoxSize, angle, 1 << LayerMask.NameToLayer("Monster"), 1 << LayerMask.NameToLayer("Boss"));

        return collider;
    }

    private IEnumerator IE_DashAttack(Tweener _tween)
    {
        yield return _tween.WaitForCompletion();

        DashAttackCheckCollider();
    }
    #endregion
}
