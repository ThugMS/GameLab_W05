using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMonster : BaseMonster
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    protected Vector3 m_targetPos;
    #endregion

    #region PublicMethod
    public override void Patrol()
    {

        if (Vector2.Distance(transform.position, base.targetPatrolPos) < 0.2f)
        {
            base.m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                m_timer = m_patrolTime;
                targetPatrolPos = base.getPatrolPos();
            }
        }


        m_targetPos = new Vector2(UnityEngine.Random.Range(m_initialPosition.x - base.m_range/2, m_initialPosition.x + base.m_range / 2),
                UnityEngine.Random.Range(m_initialPosition.y - base.m_range/2, m_initialPosition.y + base.m_range/2));

    }

    public override void Pursuit()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
    }

    protected override IEnumerator IE_KnockBack()
    {
        return base.IE_KnockBack();
    }



    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.DamagePlayer(collision.gameObject);
    }

    #endregion

    #region PrivateMethod
    #endregion
}