using JetBrains.Annotations;
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

    private float m_dis = 20f;
    private Animator m_animator;

    [Header("Status")]
    [SerializeField] private ProjectileType m_projectileType = ProjectileType.None;
    [SerializeField] private float m_lifeTime = 1.0f;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private Vector2 m_dir = Vector2.zero;
    [SerializeField] private float m_power = 1f;

    [Header("ZigZag")]
    [SerializeField] private float m_interval = 1.5f;
    [SerializeField] private float m_height = 1f;
    
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
        transform.position = PlayerManager.instance.GetPlayer().transform.position;
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_layerMask = LayerMask.GetMask("Monster", "Boss");
        m_line = GetComponent<LineRenderer>();
        m_animator = GetComponent<Animator>();
        

        transform.rotation = Quaternion.identity;

        CurvedLinePoint[] allchild = GetComponentsInChildren<CurvedLinePoint>();
        foreach(var child in allchild)
        {
            m_points.Add(child.gameObject);
        }
        
        StartCoroutine(nameof(IE_Destroy));
        CheckProjectileType();
        gameObject.SetActive(true);

        transform.SetParent(PlayerManager.instance.GetPlayer().transform);
    }



    private void Update()
    {
        for(int i = 0; i < m_points.Count - 1; i++)
        {
            m_points[i].GetComponent<CurvedLinePoint>().SetCollider(m_points[i + 1]);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Destroy()
    {
        yield return new WaitForSeconds(m_lifeTime);

        m_animator.Play("Brimstone");
    }


    private void CheckProjectileType()
    {
        switch (m_projectileType)
        {
            case ProjectileType.None:
                NoneType();
                break;

            case ProjectileType.Planet:
                PlanetType();
                break;

            case ProjectileType.Zigzag:
                ZigZagType();
                break;
        }
    }

    private void NoneType()
    {
        for (int i = 0; i < m_points.Count; i++)
        {
            if (i == m_points.Count - 1)
            {
                Vector3 pos = new Vector3(m_dir.x, m_dir.y, 0) * m_dis;
                m_points[i].transform.localPosition = pos;
                break;
            }

            m_points[i].transform.localPosition = Vector3.zero;
        }
    }

    private void PlanetType()
    {
        float angle = Vector2.SignedAngle(Vector2.right, m_dir.normalized);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        m_points[0].transform.localPosition = new Vector3(1, 0, 0);
        m_points[1].transform.localPosition = new Vector3(0, 1.5f, 0);
        m_points[2].transform.localPosition = new Vector3(-1.5f, 0, 0);
        m_points[3].transform.localPosition = new Vector3(0, -2, 0);
        m_points[4].transform.localPosition = new Vector3(2, 0, 0);
        m_points[5].transform.localPosition = new Vector3(m_dis, 0, 0);
    }

    private void ZigZagType()
    {
        float angle = Vector2.SignedAngle(Vector2.right, m_dir.normalized);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        m_points = new List<GameObject>();

        int len = (int)(m_dis / m_interval) + 1;
        int check = -1;

        for(int i = 0; i < len; i++)
        {
            check *= -1;    
            GameObject obj = (GameObject)Instantiate(Resources.Load(AttackResouceStore.ATTACK_BRIMSTONE_POINT), transform.position, Quaternion.identity, transform);
            if (i == 0)
            {
                obj.transform.localPosition = new Vector3(1, 0, 0);  
            }
            else if(i == len - 1)
            {
                obj.transform.localPosition = new Vector3(m_dis, 1, 0);
            }
            else
            {
                obj.transform.localPosition = new Vector3(i * m_interval, m_height * check, 0);
            }
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
