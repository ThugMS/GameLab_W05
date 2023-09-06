using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : BaseMonster
{
    #region PublicVariables

    #endregion

    #region PrivateVariables
    private Vector3 m_targetPos;
    [SerializeField] private float m_patrolRange;
    #endregion

    #region PublicMethod


    public override void Persuit()
    {
            base.m_agent.SetDestination(base.m_playerObj.transform.position);
    }



    #endregion

    #region PrivateMethod
    #endregion

}