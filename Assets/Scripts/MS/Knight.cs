using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [Header("Attack")]
    [SerializeField] private Vector2 m_boxSize;
    [SerializeField] private float m_offset = 0.5f;
    [SerializeField] private float m_range = 2f;

    [Header("Ability")]
    [SerializeField] private float m_dashDis;
    #endregion

    #region Test
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 attackDir = m_Direction.normalized * (m_offset + m_boxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        Gizmos.DrawWireCube(attackPos, m_boxSize);
    }
    #endregion
    #region PublicMethod
    protected override void Attack()
    {
        Collider2D[] collider = CheckCollider();
    }

    protected override void Ability()
    {
        Dash();
    }

    protected override void SetStatus()
    {
        m_power = 5f;
    }
    #endregion

    #region PrivateMethod
    private Collider2D[] CheckCollider()
    {
        Collider2D[] collider = null;

        Vector2 attackDir = m_Direction.normalized * (m_offset + m_boxSize.x / 2);
        Vector3 attackPos = transform.position + new Vector3(attackDir.x, attackDir.y, 0);

        float angle = Vector2.Angle(Vector2.right, m_Direction.normalized);
       
        collider = Physics2D.OverlapBoxAll(attackPos, m_boxSize, angle, 1 << LayerMask.NameToLayer("Monster"), 1 << LayerMask.NameToLayer("Boss"));

        return collider;
    }

    private void Dash()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, m_Direction, m_dashDis, 1<<LayerMask.NameToLayer("Ground"), 1<<LayerMask.NameToLayer("Monster"), 1<<LayerMask.NameToLayer("Boss"));

        
    }
    #endregion
}
