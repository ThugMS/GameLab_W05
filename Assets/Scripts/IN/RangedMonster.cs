using System.Collections;
using System.Collections.Generic;
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
    private bool isAttacking = false;
    #region PublicMethod

    public override void Persuit()
    {
        Vector3 playerDetection = detectingPlayer().position - transform.position;

        if (playerDetection.magnitude < base.m_range && playerDetection.magnitude > m_attackRange * .8f)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
        }
        else if (playerDetection.magnitude < m_attackRange / 2)
        {
            Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, m_speed * Time.deltaTime);
            
        }
        else if (playerDetection.magnitude < m_attackRange)
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
            GameObject bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = direction * m_bulletSpeed;
        }

        isAttacking = false;
        yield return null;

    }

    #endregion
}