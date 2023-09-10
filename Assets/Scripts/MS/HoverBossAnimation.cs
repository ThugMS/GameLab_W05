using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBossAnimation : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private HoverBoss m_boss;

    [Header("Skill")]
    [SerializeField] private GameObject m_skill;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_boss = transform.GetComponentInParent<HoverBoss>();
    }

    public void Attack()
    {
        m_boss.AttackPlayer();
    }

    public void Skill()
    {
        GameObject player = PlayerManager.instance.GetPlayer();

        Instantiate(m_skill, player.transform.position, Quaternion.identity);
    }

    public void EndAttack()
    {
        m_boss.EndAttack();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
