using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMonster : RangedMonster
{
    #region PublicVariables


    #endregion

    #region PrivateVariables
    private LineRenderer laserLine;
    [SerializeField] private float laserTime;
    [SerializeField] private float laserDamage;
    private float timer;
    #endregion

    #region PublicMethod
    protected override IEnumerator IE_Attack()
    {
        laserLine = GetComponent<LineRenderer>();
        isAttacking = true;
        timer = laserTime;
        Vector3 playerLastDirection = (base.m_playerObj.transform.position - transform.position).normalized;
        laserLine.positionCount = 2;
        laserLine.enabled = true;
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, transform.position + playerLastDirection * 100);

        if (base.m_playerObj != null)
        {
            while (timer > 0)
            {
                laserLine.SetPosition(0, transform.position);
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, playerLastDirection, m_attackRange);
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        Debug.Log("hurt");
                        // hitInfo.collider.gameObject.GetComponent<Player>().GetDamage(laserDamage);
                    }
                }
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame(); // Add this line
            }
        }
        laserLine.enabled = false;
        isAttacking = false;
        yield return null;
    }

    #endregion

    #region PrivateMethod
    #endregion
}