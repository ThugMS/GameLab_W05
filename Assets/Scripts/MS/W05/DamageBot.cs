using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageBot : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_text;
    [SerializeField] private GameObject m_panel;

    [SerializeField] private bool m_isdealing = false;
    [SerializeField] private bool m_getAttack = false;
    [SerializeField] private float m_damage = 0f;
    [SerializeField] private float m_time = 0.0f;
    [SerializeField] private float m_stopTime = 1f;
    #endregion

    #region PublicMethod
    private void Update()
    {
        if (m_isdealing == true)
        {
            m_time += Time.deltaTime;
            
        }

        if(m_getAttack == true)
        {
            StopCoroutine(nameof(IE_StopDamage));
            StartCoroutine(nameof(IE_StopDamage));
            m_getAttack = false;
        }
        
        m_panel.GetComponent<TextMeshPro>().text = GetText();
    }

    public void ShowDamage(float _damage)
    {   
        if(m_isdealing == false)
        {
            m_isdealing = true;
            m_damage = 0f;
            m_time = 0f;
        }
        m_damage += _damage;
        m_getAttack = true;

        GameObject obj = Instantiate(m_text, transform.position, Quaternion.identity);
        obj.GetComponent<DamagerText>().InitSetting(_damage.ToString());
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_StopDamage()
    {
        yield return new WaitForSeconds(m_stopTime);

        m_isdealing = false;
        m_time -= m_stopTime;

        if (m_time < 0f)
            m_time = 0;
    }

    private string GetText()
    {
        string str = "공격시간   :   " + m_time.ToString() +
            "\n총 데미지   :   " + m_damage.ToString() +
            "\n초당 데미지   :   " + (Mathf.Floor((m_damage / m_time)*100f)/100f).ToString();

        return str;
    }
    #endregion
}
