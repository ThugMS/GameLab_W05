using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeMonster : BaseMonster
{
    #region PublicVariables
    public Vector3 m_initialPosition;

    #endregion

    #region PrivateVariables
    private Vector3 m_targetPos;
    [SerializeField] private float m_patrolRange;
    #endregion

    #region PublicMethod

    public override void Persuit()
    {
        Vector3 playerDetection = detectingPlayer().position - transform.position;
        if (playerDetection.magnitude < base.m_range)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerObj.transform.position, m_speed * Time.deltaTime);
        }
        else
        {
            Move();
        }
    }

    public override void Move()
    {
        if (Vector3.Distance(m_initialPosition, transform.position) < 0.2f)
        {
            m_targetPos = new Vector2(UnityEngine.Random.Range(m_initialPosition.x - m_patrolRange, m_initialPosition.x + m_patrolRange), 
                UnityEngine.Random.Range(m_initialPosition.y - m_patrolRange, m_initialPosition.y + m_patrolRange));
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, m_targetPos, base.m_speed * Time.deltaTime);
        }
    }
    #endregion

    #region PrivateMethod
    #endregion

}