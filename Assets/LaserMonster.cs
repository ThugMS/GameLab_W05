using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMonster : RangedMonster
{
    #region PublicVariables


    #endregion

    #region PrivateVariables
    private LineRenderer laserLine;
    [SerializeField] private float laserWaitTime;
    [SerializeField] private float laserTime;
    [SerializeField] private float laserDamage;
    private float timer;
    private bool hasHitPlayer; // Flag to track if the raycast has hit the player

    #endregion

    #region PublicMethod
    protected override IEnumerator IE_Attack()
    {
        laserLine = GetComponent<LineRenderer>();
        isAttacking = true;
        timer = laserWaitTime;
        Vector3 playerLastDirection = (base.m_playerObj.transform.position - transform.position).normalized;
        laserLine.positionCount = 2;
        laserLine.enabled = true;
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, transform.position + playerLastDirection * 1);

        hasHitPlayer = false;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, playerLastDirection, m_attackRange, base.m_detectingLayer);

        while (timer > 0)
        {
            float lineLength = Mathf.Lerp(1, 25, 1 - (timer / laserTime)); 
            laserLine.SetPosition(1, transform.position + playerLastDirection * lineLength);
            timer -= Time.deltaTime;
        }

        laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
        timer = laserTime;

        base.TransitionToState(MonsterState.Stop);

        if (base.m_playerObj != null)
        {
            while (timer > 0)
            {
                laserLine.SetPosition(0, transform.position);
                if (!hasHitPlayer)
                {
                    if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.CompareTag("Player"))
                        {
                            Debug.Log("hurt");
                            hitInfo.collider.gameObject.GetComponent<Player>().GetDamage(laserDamage);
                            hasHitPlayer = true;
                        }
                    }
                }
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        laserLine.enabled = false;
        base.TransitionToState(MonsterState.Patrol);
        isAttacking = false;
        yield return null;
    }

    #endregion

    #region PrivateMethod
    #endregion
}