using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEditor.Progress;

public class WizardAbility : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private int m_enempLayerMask;
    private CircleCollider2D m_collider;
    private int m_enemyLayerMask;

    [Header("Explosion")]
    [SerializeField] private float m_explosionTime = 2f;
    [SerializeField] private float m_power = 3f;
    [SerializeField] private Dictionary<string, Collider2D> m_targets;
    [SerializeField] private bool m_isExplo = false;
    #endregion

    #region PublicMethod
    private void Start()
    {
        TryGetComponent<CircleCollider2D>(out m_collider);
        StartCoroutine(nameof(IE_Explo));
        m_enemyLayerMask = LayerMask.GetMask("Monster", "Boss");
        m_targets = new Dictionary<string, Collider2D>();
    }
    #endregion

    #region PrivateMethod
    private void Explosion()
    {   
        m_isExplo = true;

        foreach(var item in m_targets.Values)
        {
            BaseMonster monster;

            item.transform.TryGetComponent<BaseMonster>(out monster);
            monster.getDamage(m_power);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((m_enemyLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            m_targets.Add(collision.gameObject.name, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   
        if(m_isExplo == true)
        {
            return;
        }

        if ((m_enemyLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            m_targets.Remove(collision.gameObject.name);
        }
    }

    private IEnumerator IE_Explo()
    {
        yield return new WaitForSeconds(m_explosionTime);
        Explosion();
    }
    #endregion
}
