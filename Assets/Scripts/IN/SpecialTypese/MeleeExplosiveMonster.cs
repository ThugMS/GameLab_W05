using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeExplosiveMonster : MeleeMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_explosionPrefab;
    [SerializeField] private float m_explosionWaitTime;
    #endregion

    #region PublicMethod
    public override void Update()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                if (detectPlayer())
                {
                    TransitionToState(MonsterState.Pursuit);
                }
                Patrol();
                break;
            case MonsterState.Pursuit:
                if (!detectPlayer())
                {
                    Patrol();
                    
                }
                else
                {
                    if (Vector2.Distance(transform.position, base.m_playerObj.transform.position) < 1f)
                    {
                        
                        StartCoroutine(IE_Attack());
                        TransitionToState(MonsterState.Dead);
                    }
                    Pursuit();
                }
                break;
        }
    }

    protected IEnumerator IE_Attack()
    {
        yield return new WaitForSeconds(m_explosionWaitTime);
        GameObject bullet = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield return null;
    }
    #endregion

    #region PrivateMethod

    #endregion
}