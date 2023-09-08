using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
        laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
        hasHitPlayer = false;
        
        base.TransitionToState(MonsterState.Stop);

        while (timer > 0)
        {
            float t = 1f - (timer / laserWaitTime); 
            float laserWidth = Mathf.Lerp(0f, 1f, t); 
            laserLine.startWidth = laserWidth;
            laserLine.endWidth = laserWidth;
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = laserTime;
        if (base.m_playerObj != null)
        {
            while (timer > 0)
            {
                laserLine.SetPosition(0, transform.position);
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, playerLastDirection, m_attackRange, base.m_detectingLayer);
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
        print("end");
        laserLine.SetPosition(1, transform.position);
        base.TransitionToState(MonsterState.Patrol);
        isAttacking = false;

        yield return new WaitForSeconds(laserTime);
        yield return null;
    }

    protected void laserWait(Vector3 playerLastDirection)
    {
        while (timer > 0)
        {
            float t = 1f - (timer / laserWaitTime);
            float laserWidth = Mathf.Lerp(0f, 1f, t);
            laserLine.startWidth = laserWidth;
            laserLine.endWidth = laserWidth;
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
            timer -= Time.deltaTime;
        }
    }

    #endregion

    #region PrivateMethod
    #endregion
}