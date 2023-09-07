using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeExplosiveMonster : RangedMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod

    protected override IEnumerator IE_Attack()
    {

        base.isAttacking = true;
        yield return new WaitForSeconds(base.m_attackTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }

        base.isAttacking = false;
        yield return null;
    }
    #endregion

    #region PrivateMethod

    #endregion
}