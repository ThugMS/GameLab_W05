using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedBoss : BaseMonster
{
    enum Pattern
    {
        RushToPlayer,
        SingleFireball,
        CircleFireball
    }

    protected Vector3 m_targetPos;
    private bool m_onSkill;
    private bool m_restMode;
    private bool m_isMove;
    private Pattern m_currentPattern;
    
    [Header("Ranged Boss Related")]
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_bulletSpeed;
    [SerializeField] private int m_rushSpeed;
    [SerializeField] private int m_circleBulletDistance;
    private Vector3 m_MovePositionToTarget;
    
    [Header("Ranged Boss Skill")]
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int ShortAttack = Animator.StringToHash("ShortAttack");
    private static readonly int Rush = Animator.StringToHash("Rush");
    private List<Pattern> patterns = new()
    {
        Pattern.RushToPlayer,
        Pattern.SingleFireball,
        Pattern.SingleFireball,
        Pattern.CircleFireball,
        Pattern.CircleFireball,
    };

    public void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        m_animator = GetComponentInChildren<Animator>();
        m_playerObj ??= GameObject.FindGameObjectWithTag("Player");
        m_onSkill = false;
        m_restMode = true;
    }

    protected override void stateUpdate()
    {
        // 액션 관리
        if (!m_onSkill)
        {
            m_onSkill = true;
            if (m_restMode)
            {
                StartCoroutine(nameof(IERest));
            }
            else
            {
                switch (m_currentPattern)
                {
                    case Pattern.RushToPlayer:
                        StartCoroutine(RushToPlayer(3));
                        break;
                    case Pattern.SingleFireball:
                        m_animator.SetBool(ShortAttack, true);
                        break;
                    case Pattern.CircleFireball:
                        m_animator.SetBool(Attack1, true);
                        break;
                }
            }
        }

        // 이동 시, 플레이어 방향으로 flip
        TowardPlayer();
        if (m_isMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_MovePositionToTarget, m_speed * Time.deltaTime);
        }
    }
    
    IEnumerator IERest()
    {
        m_animator.Play("RangedBossIdle");
        
        // 다음 공격 패턴 지정
        m_currentPattern =  patterns[Random.Range(0, patterns.Count)];
        yield return new WaitForSeconds(.5f);
        
        // 1초간 추적
        m_animator.SetBool(Move, true);
        yield return IEPatrol(1);
        m_animator.SetBool(Move, false);
        
        m_onSkill = false;
        m_restMode = false;
    }
    
    IEnumerator IEPatrol(int second, bool isRush = false)
    {
        float timer = second;
        m_isMove = true;

        do
        {
            m_MovePositionToTarget = m_playerObj.transform.position;
            yield return new WaitForSeconds(.5f);
            timer -= .5f;
        } while (timer > 0);
        m_isMove = false;
        yield return null;
    }

    IEnumerator RushToPlayer(int second)
    {
        Debug.Log("Rush Start");
        int backup_Speed = m_speed;
        m_speed = m_rushSpeed;
        
        m_animator.SetBool(Rush, true);
        yield return IEPatrol(second);
        m_speed = backup_Speed;
        EndAttackAnimation();
        
        Debug.Log("Rush End");
    }

    public void InstantiateSingleBullet()
    {
        var dir = transform.position - m_playerObj.transform.position;
        var rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
        var obj = Instantiate(m_bulletPrefab, GetBulletSpawnPos(), Quaternion.Euler(0, 0, rot));
        obj.GetComponent<RangedBossBullet>().Init(m_bulletSpeed);
    }
    
    public void InstantiateCircleBullet()
    {
        for(int i =0 ; i < m_circleBulletDistance ; i++) 
        {
            GameObject obj = Instantiate(m_bulletPrefab, GetBulletSpawnPos(), quaternion.identity);
            var radian = Mathf.PI * i * 2 / m_circleBulletDistance;
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_bulletSpeed * Mathf.Cos(radian),
                m_bulletSpeed * Mathf.Sin(radian)));
            obj.transform.Rotate(new Vector3(0,0,360 * i / m_circleBulletDistance));
                            
            obj.GetComponent<RangedBossBullet>().Init(m_bulletSpeed);
        } 
    }
    
    private Vector3 GetBulletSpawnPos()
    {
        var pos = transform.position;
        var dir = new Vector3(pos.x + (m_spriteRenderer.flipX ? -1f : 1f), pos.y, pos.z);
        return dir;
    }
    
    protected override void Patrol()
    {
    }

    protected override void Pursuit()
    {
    }

    protected override void Attack()
    {
        
    }

    public void EndAttackAnimation()
    {
        m_animator.SetBool(Rush, false);
        m_animator.SetBool(ShortAttack, false);
        m_animator.SetBool(Attack1, false);
        m_onSkill = false;
        m_restMode = true;
    }
    
    public override void getDamage(float _damage)
    {
        // m_animator.Play("RangedBossGetHit");
        Health -= _damage;

        if (Health <= 0)
        {
            // [TODO] 공격 추가?
            m_isMove = false;
            m_animator.Play("RangedBossDead");
            //이 후 죽음은 애니메이션 재생 후, OnStateExit()에서 ExecuteDeadAfterAnimation() 호출하여 종료
        }
    }

    public override void getDamage(float _damage, float knockbackPower)
    {
        // m_animator.Play("RangedBossGetHit");
        Health -= _damage;

        if (Health <= 0)
        {
            // [TODO] 공격 추가?
            m_isMove = false;
            m_animator.Play("RangedBossDead");
            //이 후 죽음은 애니메이션 재생 후, OnStateExit()에서 ExecuteDeadAfterAnimation() 호출하여 종료
        }
    }

    public void ExecuteDeadAfterAnimation()
    {
        base.Dead();
    }

    void TowardPlayer()
    {
        m_spriteRenderer.flipX = (m_playerObj.transform.position.x - transform.position.x) < 0;
    }
}
