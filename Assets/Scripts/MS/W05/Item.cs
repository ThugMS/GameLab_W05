using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region PublicVariables
    public ItemType m_type;
    #endregion

    #region PrivateVariables
    [SerializeField] private GameObject m_item;
    [SerializeField] private GameObject m_electirc;
    [SerializeField] private bool m_canItemEat = true;

    private int m_playerLayer;
    private float m_returnTime = 3f;
    #endregion

    #region PublicMethod
    private void Start()
    {
        m_playerLayer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == m_playerLayer)
        {
            if(m_canItemEat == false)
            {
                return;
            }

            GiveItem(collision.gameObject.GetComponent<IssacPlayer>());

            m_canItemEat = false;
            m_item.SetActive(false);

            StartCoroutine(nameof(IE_ReturnItem));
        }
    }

    private void GiveItem(IssacPlayer _obj)
    {
        switch (m_type) 
        {
            case ItemType.AttackSpeedUp:
                _obj.SetAttackSpeed(-0.3f);
                break;

            case ItemType.AttackSpeedDown:
                _obj.SetAttackSpeed(0.3f);
                break;

            case ItemType.PowerUp:
                _obj.SetPower(3);
                break;

            case ItemType.PowerDown:
                _obj.SetPower(-3);
                break;

            case ItemType.RangeUp:
                _obj.SetRange(2);
                break;

            case ItemType.RangeDown:
                _obj.SetRange(-2);
                break;

            case ItemType.ProjectileUp:
                _obj.SetProjectileSpeed(1);
                break;

            case ItemType.ProjectileDown:
                _obj.SetProjectileSpeed(-1);
                break;

            case ItemType.Bomb:
                _obj.SetAttackType(AttackType.Bomb);
                break;

            case ItemType.Ring:
                _obj.SetAttackType(AttackType.Ring);
                break;

            case ItemType.Brimstone:
                _obj.SetAttackType(AttackType.Brimstone);
                break;

            case ItemType.Tech:
                _obj.SetAttackType(AttackType.Tech);
                break;

            case ItemType.Tear:
                _obj.SetAttackType(AttackType.Tear);
                break;

            case ItemType.ProjectileTypeNone:
                _obj.SetProjectileType(ProjectileType.None);
                break;

            case ItemType.ProjectileTypePlanet:
                _obj.SetProjectileType(ProjectileType.Planet);
                break;

            case ItemType.ProjectileTypeZigzag:
                _obj.SetProjectileType(ProjectileType.Zigzag);
                break;

            case ItemType.AttackOptionNone:
                m_electirc.GetComponent<AttackStorage>().m_isElectric = false;
                break;

            case ItemType.AttackOptionElectric:
                m_electirc.GetComponent<AttackStorage>().m_isElectric = true;
                break;
        }
    }
    
    private IEnumerator IE_ReturnItem()
    {
        yield return new WaitForSeconds(m_returnTime);

        m_item.SetActive(true);
        m_canItemEat = true;
    }
    #endregion
}
