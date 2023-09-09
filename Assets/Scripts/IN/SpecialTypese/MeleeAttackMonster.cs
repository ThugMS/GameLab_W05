using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeAttackMonster : MeleeMonster
{
    [SerializeField] private float boxCastWidth;
    [SerializeField] private float attackOffset;
    [SerializeField] private float m_attackRange;
    private Vector3 attackDir;
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod


    protected override void Pursuit()
    {
        Vector3 playerDetection = base.m_playerObj.transform.position - transform.position;

        if (playerDetection.magnitude > m_attackRange * .8f)
        {
            base.m_agent.ResetPath();

            base.m_agent.SetDestination(base.m_playerObj.transform.position);
        }
        else if (playerDetection.magnitude < m_attackRange / 2)
        {
            base.m_agent.ResetPath();

            Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
            base.m_agent.SetDestination((Vector2)transform.position + moveDirection);
        }

        if (playerDetection.magnitude < m_attackRange)
        {
            base.TransitionToState(MonsterState.Stop);
            Attack();
        }

    }


    protected override void Attack()
    {
        //m_animator.SetTrigger("Attack");
        AttackCheckCollider();
    }



    private void AttackCheckCollider()
    {
        
        attackDir =  (base.m_playerObj.transform.position - transform.position).normalized;
        string animationState;

        if (Mathf.Abs(attackDir.x) > Mathf.Abs(attackDir.y))
        {
            animationState = (attackDir.x > 0) ? "Right" : "Left";
        }
        else
        {
            animationState = (attackDir.y > 0) ? "Up" : "Down";
        }

        switch (animationState)
        {
            case "Right":
                attackDir = Vector2.right;
                break;
            case "Left":
                attackDir = Vector2.left;
                //animator.SetBool("IsMovingLeft", true);
                break;
            case "Up":
                attackDir = Vector2.up;
                //animator.SetBool("IsMovingUp", true);
                break;
            case "Down":
                attackDir = Vector2.down;
                //animator.SetBool("IsMovingDown", true);
                break;
            default:
                break;
        }

        /*
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)attackDir * attackOffset, new Vector2(boxCastWidth, boxCastWidth), 0f, attackDir);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Attack();
        }
        else
        {
            //animator.SetBool("IsAttacking", false);
        }
        */

        base.TransitionToState(MonsterState.Patrol);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)attackDir * attackOffset, new Vector3(boxCastWidth, boxCastWidth, 0.1f));
    }



    #endregion
}