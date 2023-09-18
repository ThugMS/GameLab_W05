using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Timeline;

public class DamagerText : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    private TMP_Text m_text; 
    #endregion

    #region PublicMethod
    private void Awake()
    {
        
        m_text = transform.GetComponent<TextMeshPro>();

        StartCoroutine(nameof(IE_Destroy));
    }

    public void InitSetting(string _text)
    {
        m_text.text = _text;
        transform.DOMoveY(transform.position.y + 3f, 0.2f);
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
    #endregion
}
