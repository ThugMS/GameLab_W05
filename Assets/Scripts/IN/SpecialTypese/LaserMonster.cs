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
    private Vector3 playerLastDirection;
    #endregion

    #region PublicMethod
    protected override IEnumerator IE_Attack()
    {
        InitializeLaser();
        base.m_animator.SetBool("isAttacking", true);
        base.TransitionToState(MonsterState.Stop);

        while (timer > 0)
        {
            UpdateLaserWidth();
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = laserTime;

        if (base.m_playerObj != null)
        {
            while (timer > 0)
            {
                CheckForPlayerHit();
                timer -= Time.deltaTime;
            }
        }

        EndLaserAttack();
        yield return new WaitForSeconds (laserTime);
        base.TransitionToState(MonsterState.Patrol);
        base.m_animator.SetBool("isAttacking", false);
        yield return null;
    }

    private void InitializeLaser()
    {
        laserLine = GetComponent<LineRenderer>();
        isAttacking = true;
        timer = laserWaitTime;
        playerLastDirection = (base.m_playerObj.transform.position - transform.position).normalized;
        laserLine.positionCount = 2;
        laserLine.enabled = true;
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
        hasHitPlayer = false;
    }

    private void UpdateLaserWidth()
    {
        float t = 1f - (timer / laserWaitTime);
        float laserWidth = Mathf.Lerp(0f, 1f, t);
        laserLine.startWidth = laserWidth;
        laserLine.endWidth = laserWidth;
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, transform.position + playerLastDirection * 20);
    }

    private void CheckForPlayerHit()
    {
        laserLine.SetPosition(0, transform.position);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, playerLastDirection, m_attackRange, base.m_detectingLayer);

        if (!hasHitPlayer && hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
        {
            Debug.Log("hurt");
            hitInfo.collider.gameObject.GetComponent<Player>().GetDamage(laserDamage);
            hasHitPlayer = true;
        }
    }

    private void EndLaserAttack()
    {
        print("end");
        laserLine.SetPosition(1, transform.position);
        isAttacking = false;
        timer = laserWaitTime;

        while (timer > 0)
        {
            UpdateLaserWidth();
            timer -= Time.deltaTime;
            playerLastDirection = Vector2.zero;
        }
    }

    #endregion

    #region PrivateMethod
    #endregion
}