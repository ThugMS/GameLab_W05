using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    public bool isStartCounting = false;
    [SerializeField] GameObject realBomb;
    public float m_explosionSize;
    #endregion

    #region PublicMethod

    protected override void Update()
    {
        if (isStartCounting == true)
        {
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                StartCoroutine(nameof(IE_playBomb));
            }
        }
    }

    private IEnumerator IE_playBomb()
    {
        Bullet Bomb = Instantiate(realBomb, transform.position, Quaternion.identity).GetComponent<Bullet>();
        Bomb.m_limitTime = this.m_limitTime;
        Bomb.m_damage = this.m_damage;
        Bomb.transform.localScale = new Vector3(m_explosionSize, m_explosionSize, m_explosionSize);

        yield return new WaitForFixedUpdate();

        Destroy(gameObject);

    }



    #endregion

    #region PrivateMethod
    #endregion
}