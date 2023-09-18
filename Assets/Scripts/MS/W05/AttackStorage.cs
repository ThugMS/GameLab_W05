using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStorage : MonoBehaviour
{
    #region PublicVariables
    public bool m_isElectric = false;
    #endregion

    #region PrivateVariables
    [SerializeField] LineRenderer m_line;
    [SerializeField] Vector3[] m_points;
    [SerializeField] List<Vector2> m_points2D = new List<Vector2>();
    [SerializeField] Vector2[] m_pointsArr;
    [SerializeField] EdgeCollider2D m_edgeCollider;

    private int m_layerMask;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_layerMask = LayerMask.GetMask("Monster", "Boss");
    }
    private void Update()
    {
        if(m_isElectric == true)
        {
            Tear[] objs = gameObject.GetComponentsInChildren<Tear>();
            m_points = new Vector3[objs.Length];
            m_pointsArr = new Vector2[objs.Length]; 

            for (int i=0;i<objs.Length; i++)
            {
                m_points[i] = objs[i].transform.position;
                m_pointsArr[i] = objs[i].transform.position;

                m_points2D.Add(objs[i].transform.position);
            }
            m_line.positionCount = m_points.Length;
            m_line.SetPositions(m_points);

            m_edgeCollider.points = m_pointsArr;
        }
        
    }
    #endregion

    #region PrivateMethod
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((m_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<DamageBot>().ShowDamage(1);           
        }
    }
    #endregion
}
