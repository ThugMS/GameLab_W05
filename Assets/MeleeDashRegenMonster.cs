using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDashRegenMonster : MeleeDashMonster
{
    #region PublicVariables
    public GameObject[] objectArr;

    #endregion

    #region PrivateVariables
    [SerializeField] private bool isParent;
    #endregion

    #region PublicMethod



    public void getDamage(float _damage, float knockbackPower)
    {
        TransitionToState(MonsterState.Knockback);
        Health -= _damage;
        Vector2 moveDirection = (transform.position - m_playerObj.transform.position).normalized;
        if (m_agent.isActiveAndEnabled == true)
        {
            m_agent.SetDestination((Vector2)transform.position + moveDirection);
        }
        StartCoroutine(IE_KnockBack(knockbackPower));
        if (Health <= 0)
        {
            if (isParent)
            {
                turnChild(true);
            } else
            {
                checkDone();
            }
            gameObject.SetActive(false);
        }
    }

    private void turnChild(bool value)
    {
        foreach (GameObject child in objectArr)
        {
          child.gameObject.SetActive(value);
        }
    }


    public void checkDone()
    {
        Transform parentTransform = transform.parent;

        int totalChildCount = 0;

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);

            if (child.gameObject.activeInHierarchy)
            {
                totalChildCount++;
            }
        }

        if (totalChildCount == 1)
        {
            Dead();
        }
    }

    #endregion

    #region PrivateMethod
    #endregion
}