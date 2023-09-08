using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseMonster;

public class MonsterStateManager : MonoBehaviour
{
    /*
    public static MonsterStateManager Instance { get; private set; }

    public void getStateFromManager(BaseMonster monsterObject)
    {
        switch (monsterObject.m_currentState)
        {        case BaseMonster.MonsterState.Patrol:
                if (detectPlayer())
                {
                    monsterObject.m_currentState = MonsterState.Pursuit; break;
                }
                break;
            case BaseMonster.MonsterState.Pursuit:
                if (!detectPlayer())
                {
                    monsterObject.m_currentState = MonsterState.Patrol; break;
                }
                else
                {
                    monsterObject.m_currentState = MonsterState.Pursuit; break;
                }
        }
    }
    public virtual bool detectPlayer()
    {
        return canSeePlayer() && playerWithinRange();
    }

    public virtual bool canSeePlayer()
    {
        Vector2 directionToPlayer = m_playerObj.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, m_detectingLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    public virtual bool playerWithinRange()
    {
        if (Vector2.Distance(transform.position, m_playerObj.transform.position) < m_range)
        {
            return true;
        }
        return false;
    }



    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    #endregion
    */
}