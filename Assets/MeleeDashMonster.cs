using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDashMonster : MeleeMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    [SerializeField] private float dashDistance;
    #endregion

    #region PublicMethod
    public override void Pursuit()
    {
        StartCoroutine(dash());
    }

    
    #endregion

    #region PrivateMethod
    
    IEnumerator dash()
    {
        base.m_agent.isStopped = true;
        base.m_rb.AddForce((transform.position - base.m_playerObj.transform.position).normalized * dashDistance);
        while (base.m_rb.velocity.magnitude < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        base.m_rb.velocity = Vector2.zero;
        base.m_agent.isStopped = false;
        base.TransitionToState(MonsterState.Patrol);
    }
    #endregion
}