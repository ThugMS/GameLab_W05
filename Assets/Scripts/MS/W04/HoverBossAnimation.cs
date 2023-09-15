using JetBrains.Annotations;
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

    [Header("Summon")]
    [SerializeField] private GameObject m_summon;
    [SerializeField] private Vector2[] m_spawnPos = new Vector2[3];
    [SerializeField] private float m_spawnXOffset = 3f;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_boss = transform.GetComponentInParent<HoverBoss>();

        m_spawnPos[0] = new Vector2(transform.position.x - m_spawnXOffset, transform.position.y);
        m_spawnPos[1] = new Vector2(transform.position.x, transform.position.y);
        m_spawnPos[2] = new Vector2(transform.position.x + m_spawnXOffset, transform.position.y);
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

    public void Summon()
    {   
        for(int i = 0; i < m_spawnPos.Length; i++)
        {
            Instantiate(m_summon, m_spawnPos[i], Quaternion.identity);
        }
    }

    public void EndAttack()
    {
        m_boss.EndAttack();
    }
    #endregion

    #region PrivateMethod
    #endregion
}
