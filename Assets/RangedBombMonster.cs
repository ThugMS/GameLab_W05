using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBombMonster : RangedMonster
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private int m_BombCount;
    #endregion

    protected override IEnumerator IE_Attack()
    {
        isAttacking = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 initialPlayerPosition = player.transform.position;
            Vector2 direction = (initialPlayerPosition - (Vector2)transform.position).normalized;

            base.TransitionToState(MonsterState.Stop);
            for (int i = 0; i < m_BombCount; i++)
            {
                GameObject bullet = Instantiate(m_bullet, transform.position, Quaternion.identity);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                while (bullet!=null&&Vector2.Distance(bullet.transform.position, initialPlayerPosition) > 0.2f)
                {
                    bullet.GetComponent<Bomb>().isStartCounting = true;
                    bulletRigidbody.velocity = direction * m_bulletSpeed;
                    if(bullet == null) break;
                            yield return null;
                }
                if (bullet != null)
                {
                    bulletRigidbody.velocity = Vector2.zero;
                }
                yield return new WaitForSeconds(0.25f);
                base.m_animator.SetBool("isAttacking", false);
                base.TransitionToState(MonsterState.Pursuit);
            }
        }

        isAttacking = false;
        yield return null;
    }
}
   