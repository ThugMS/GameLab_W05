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

    [SerializeField] private float m_attackRange;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private int m_bulletCount;
    private bool isAttacking = false;
    #region PublicMethod



    public override void Persuit()
    {
        Vector3 playerDetection = detectingPlayer().position - transform.position;

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
            StartCoroutine(IEAttack());
        }
    }



    #endregion

    #region PrivateMethod

    private IEnumerator IEAttack()
    {
        

        isAttacking = true;
        yield return new WaitForSeconds(base.m_attackTime);
        
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

    #endregion
}