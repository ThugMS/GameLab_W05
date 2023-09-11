using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBossSkillEffect : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private int m_playerLayerMask;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Vector2 m_attackBoxSize;
    [SerializeField] private Collider2D m_playerCol;
    [SerializeField] private float m_power = 2f;
    #endregion

    #region Test
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, m_attackBoxSize);
    }
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_playerLayerMask = LayerMask.GetMask("Player");
    }

    public void Attack()
    {
        CheckCollider();

        if (m_playerCol == null)
            return;

        Player player;
        m_playerCol.TryGetComponent<Player>(out player);

        player.GetDamage(m_power);
    }
    #endregion

    #region PrivateMethod
    private void CheckCollider()
    {
        m_playerCol = null;

        m_playerCol = Physics2D.OverlapBox(transform.position, m_attackBoxSize, 0, m_playerLayerMask);
    }
    #endregion
}
