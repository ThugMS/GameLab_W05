using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageBot : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_text;
    #endregion

    #region PublicMethod
    public void ShowDamage(float _damage)
    {
        GameObject obj = Instantiate(m_text, transform.position, Quaternion.identity);
        obj.GetComponent<DamagerText>().InitSetting(_damage.ToString());
    }
    #endregion

    #region PrivateMethod
    #endregion
}
