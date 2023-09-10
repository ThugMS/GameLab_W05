using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedMonster : BaseMonster
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion
    [Header("RangedAttack")]
    [SerializeField] protected float m_attackRange;
    [SerializeField] protected GameObject m_bullet;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private int m_bulletCount;
    [SerializeField] protected int m_attackTime;

    protected bool isAttacking = false;
    #region PublicMethod

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
            Attack();
        }

    }

    protected override void Attack()
    {
        if (isAttacking == false)
        {
            StartCoroutine(nameof(IE_Attack));
        }
    }

    #endregion

    #region PrivateMethod
    protected virtual void Start()
    {
        base.init();
    }

    protected override void stateUpdate()
    {
        switch (m_currentState)
        {
            case MonsterState.Patrol:
                base.m_animator.SetFloat("X", (targetPatrolPos - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (targetPatrolPos - (Vector3)transform.position).y);
                if (canSeePlayer() && playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                base.m_animator.SetFloat("X", (base.m_playerObj.transform.position - (Vector3)transform.position).x);
                base.m_animator.SetFloat("Y", (base.m_playerObj.transform.position - (Vector3)transform.position).y);
                if (!canSeePlayer() || !playerWithinRange())
                {
                    Patrol();
                }
                else
                {
                    Pursuit();
                }
                break;
        }

    }
    protected virtual IEnumerator IE_Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(m_attackTime);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            for (int i = 0; i < m_bulletCount; i++)
            {
                base.TransitionToState(MonsterState.Stop);
                base.m_animator.SetBool("isAttacking", true);
                GameObject bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction * m_bulletSpeed;
                yield return new WaitForSeconds(.25f);
                base.m_animator.SetBool("isAttacking", false);
                base.TransitionToState(MonsterState.Pursuit);

            }
        }

        isAttacking = false;
        yield return null;

    }

    protected override void Patrol()
    {
        m_agent.ResetPath();
        m_agent.SetDestination(targetPatrolPos);
        if (Vector2.Distance(transform.position, targetPatrolPos) < 0.2f)
        {
            base.m_patrolTimer -= Time.deltaTime;
            if (base.m_patrolTimer < 0)
            {
                base.m_patrolTimer = m_patrolTime;
                targetPatrolPos = getPatrolPos();
            }
        }
    }

   

    #endregion
}