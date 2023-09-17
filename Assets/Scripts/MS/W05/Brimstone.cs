using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Brimstone : MonoBehaviour
{
    #region PublicVariables
    public List<GameObject> m_points = new List<GameObject>();
    #endregion

    #region PrivateVariables
    private Rigidbody2D m_rigidbody;
    private int m_layerMask;
    private LineRenderer m_line;

    [Header("Status")]
    [SerializeField] private ProjectileType m_projectileType = ProjectileType.None;
    [SerializeField] private float m_lifeTime = 1.0f;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Vector2 m_dir = Vector2.zero;
    [SerializeField] private float m_power = 1f;
    [SerializeField] private float m_size;
    #endregion

    #region PublicMethod
    public void InitSetting(ProjectileType _type, float _lifeTime, Vector2 _dir, float _power)
    {
        m_projectileType = _type;
        m_lifeTime = _lifeTime;
        m_dir = _dir;
        m_power = _power;
    }

    public void FixedUpdate()
    {
        Debug.Log(transform.position);
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_layerMask = LayerMask.GetMask("Monster", "Boss");
        m_line = GetComponent<LineRenderer>();
        transform.rotation = Quaternion.identity;

        CurvedLinePoint[] allchild = GetComponentsInChildren<CurvedLinePoint>();
        foreach(var child in allchild)
        {
            m_points.Add(child.gameObject);
        }
        
        StartCoroutine(nameof(IE_Destroy));
        SetShape();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        for(int i = 0; i < m_points.Count - 1; i++)
        {
            m_points[i].GetComponent<CurvedLinePoint>().SetCollider(m_points[i + 1]);
        }
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }

    private void SetShape()
    {
        switch(m_projectileType)
        {
            case ProjectileType.None:
                ShapeNone();
                break;
        }
    }

    private void ShapeNone()
    {
        for (int i = 0; i < m_points.Count; i++)
        {
            if (i == m_points.Count - 1)
            {
                Vector3 pos = new Vector3(m_dir.x, m_dir.y, 0) * 10f;
                m_points[i].transform.localPosition = pos;
                break;
            }

            m_points[i].transform.localPosition = Vector3.zero;
        }
    }

    private void GenerateCollider()
    {
        MeshCollider col = GetComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        m_line.BakeMesh(mesh);
        col.sharedMesh = mesh;

        col.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((m_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<DamageBot>().ShowDamage(m_power);
        }
    }

    #endregion
}
