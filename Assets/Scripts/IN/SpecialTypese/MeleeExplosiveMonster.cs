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
    private bool isDeadOn = false;
    #endregion

    #region PublicMethod
    protected override void Pursuit()
    {
        if (Vector2.Distance(transform.position, base.m_playerObj.transform.position) < 2f)
        {
            StartCoroutine(IE_Attack());
        }
        else
        {
            m_agent.SetDestination(m_playerObj.transform.position);

        }
    }

    protected IEnumerator IE_Attack()
    {
        yield return new WaitForSeconds(m_explosionWaitTime);
        GameObject bullet = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        bullet.transform.localScale = new Vector3(m_explosionSize, m_explosionSize, m_explosionSize);
        TransitionToState(MonsterState.Dead);
        if (!isDeadOn)
        {
            isDeadOn = true;
            Dead();
        }
        yield return new WaitForFixedUpdate();
    }


    public override void getDamage(float _damage)
    {
        Health -= _damage;
        StartCoroutine(nameof(IE_TweenDamage));
        if (Health <= 0)
        {

            if (!isDeadOn)
            {
                isDeadOn = true;
                StartCoroutine(nameof(IE_PlayDyingEffect));
            }

        }
    }
    #endregion

    #region PrivateMethod

    #endregion
}