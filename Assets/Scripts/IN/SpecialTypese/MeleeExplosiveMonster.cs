using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class MeleeExplosiveMonster : MeleeMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_explosionPrefab;
    [SerializeField] private float m_explosionSize;
    [SerializeField] private float m_explosionWaitTime;
    #endregion

    #region PublicMethod
    protected override void Pursuit()
    {
        if (Vector2.Distance(transform.position, base.m_playerObj.transform.position) < 2f)
        {
            StartCoroutine(IE_Attack());
        } else
        {
            m_agent.SetDestination(m_playerObj.transform.position);

        }
    }

    protected IEnumerator IE_Attack()
    {
        yield return new WaitForSeconds(m_explosionWaitTime);
        GameObject bullet = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(m_explosionSize, m_explosionSize,m_explosionSize);
        TransitionToState(MonsterState.Dead);
        Dead();
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
        yield return null;
    }
    #endregion

    #region PrivateMethod

    #endregion
}