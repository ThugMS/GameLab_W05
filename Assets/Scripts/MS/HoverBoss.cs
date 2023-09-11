using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverBoss : BaseMonster
{
    #region PublicVariables
    public bool m_isAttack = false;
    public bool m_canAttack = true;
    public bool m_isDead = false;
    public bool m_isStartAttackIE = false;
    #endregion

    #region PrivateVariables
    [SerializeField] private float m_attackTerm = 5f;
    [SerializeField] private Collider2D m_collider;
    private int m_playerLayerMask;

    [Header("Status")]
    [SerializeField] private float m_power = 0.5f;

    [Header("Move")]
    [SerializeField] private float m_curSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator m_bossAnimator;

    [Header("Attack")]
    [SerializeField] private float m_fadeInCoolTime = 2f;
    [SerializeField] private Vector2 m_attackBoxSize;
    [SerializeField] private Collider2D m_playerCol;
    #endregion

    #region Test
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_playerLayerMask = LayerMask.GetMask("Player");
        isBoss = true;
        init();
    }

    public void AttackPlayer()
    {
        CheckCollider();

        if (m_playerCol == null)
            return;

        Player player;
        m_playerCol.TryGetComponent<Player>(out player);

        player.GetDamage(m_power);
    }

    public void EndAttack()
    {
        m_isAttack = false;
        m_isStartAttackIE = false;
    }

    public override void init()
    {
        isOn = true;
    }

    public void Summon()
    {
        m_bossAnimator.SetTrigger("Summon");
    }

    protected override void Dead()
    {
        DeadListener?.Invoke();

        m_bossAnimator.SetTrigger("Dead");
    }
    #endregion

    #region PrivateMethod
    private void ChoicePattern()
    {
        int value = Random.Range(0, 3);

        switch(value)
        {
            case 0:
                Attack();
                break;

            case 1:
                Skill();
                break;

            case 2:
                Summon();
                break;

            default:
                break;
        }
    }

    private void AttackCheck()
    {
        if(m_isAttack == true)
        {
            return;
        }

        if (m_canAttack)
        {
            m_canAttack = false;
            m_isAttack = true;
            ChoicePattern();
        }
        else
        {
            if (m_isStartAttackIE == false)
            {
                m_isStartAttackIE = true;
                StartCoroutine(nameof(WaitAttackCoolTime));
            }
        }
    }

    private void Skill()
    {
        m_bossAnimator.SetTrigger("Skill"); 
    }

    private void CheckCollider()
    {
        m_playerCol = null;

        m_playerCol = Physics2D.OverlapBox(transform.position, m_attackBoxSize, 0, m_playerLayerMask);
    }

    //private void Attack()
    //{
        
    //}

    private void AttackTranslate()
    {
        GameObject player = PlayerManager.instance.GetPlayer();

        transform.position = player.transform.position;
    }

    private IEnumerator WaitAttackCoolTime()
    {
        yield return new WaitForSeconds(m_attackTerm);

        m_canAttack = true;
    }

    private IEnumerator FadeInCoolTime()
    {
        yield return new WaitForSeconds(m_fadeInCoolTime);

        AttackTranslate();
        m_bossAnimator.SetTrigger("FadeIn");
    }

    protected override void stateUpdate()
    {
        AttackCheck();
    }

    protected override void Patrol()
    {
        
    }

    protected override void Pursuit()
    {
        
    }

    protected override void Attack()
    {   
        if(m_bossAnimator != null)
        {
            m_bossAnimator.SetTrigger("FadeAttack");
        }
        m_collider.enabled = false;
        StartCoroutine(nameof(FadeInCoolTime));
    }
    #endregion
}
