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

    public override void Pursuit()
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

    public override void Attack()
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
                if (canSeePlayer() && playerWithinRange())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                if (!canSeePlayer() && playerWithinRange())
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
                GameObject bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = direction * m_bulletSpeed;
                yield return new WaitForSeconds(.25f);
            }
        }

        isAttacking = false;
        yield return null;

    }

    public override void Patrol()
    {
        m_agent.ResetPath();
        m_agent.SetDestination(targetPatrolPos);
        if (Vector2.Distance(transform.position, targetPatrolPos) < 0.2f)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = m_patrolTime;
                targetPatrolPos = getPatrolPos();
            }
        }
    }

    #endregion
}