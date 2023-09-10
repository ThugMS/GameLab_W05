using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// [MJ] �������� ��� �ֱ� ���� ���ҽ� �Ŵ��� ��ũ��Ʈ
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    public Dictionary<MonsterType, List<GameObject>> MonsterPrefabDict { get; private set; }
    public Dictionary<RoomType, List<GameObject>> LandscapeByRoomTypePrefabDict { get; private set; }
    
    public void Init()
    {
        InitMonster();
        InitLandscapeInRoom();
    }

    void InitMonster()
    {
        MonsterPrefabDict = new Dictionary<MonsterType, List<GameObject>>();

        int cnt = Enum.GetNames(typeof(MonsterType)).Length;

        for (int index = 0; index < cnt; index++)
        {
            var type = (MonsterType)index;
            var objList = Resources.LoadAll("Prefabs/Monsters/" + type.ToString(), typeof(GameObject))
                .Where(obj => obj is GameObject && ((GameObject)obj).tag == "Monster")
                .Select(obj => (GameObject)obj)
                .ToList();

            if (objList.Count > 0)
            {
                print(objList[0].name);
                MonsterPrefabDict.Add(type, objList);
            }
        }
    }

    void InitLandscapeInRoom()
    {
        LandscapeByRoomTypePrefabDict = new();
        
        for (int i = 0, cnt = Enum.GetNames(typeof(RoomType)).Length; i < cnt; i++ )
        {
            var type = (RoomType)i;
            var list = Resources.LoadAll($"Prefabs/Landscape/{type.ToString()}/", typeof(GameObject))
                        .Select(obj => (GameObject)obj)
                        .ToList();
            
            LandscapeByRoomTypePrefabDict.Add(type, list);
        }
    }

    public GameObject GetRandomLandscapeByType(RoomType roomType)
    {
        if (roomType == RoomType.NormalGift) roomType = RoomType.Gift;
        if (LandscapeByRoomTypePrefabDict.ContainsKey(roomType))
        {
            var values = LandscapeByRoomTypePrefabDict[roomType];
            if (values.Count > 0)
            {
                int idx = Random.Range(0, values.Count);
                return values[idx];
            }
            else
            {
                return null; 
            }
        }
        else
        {
            Debug.LogError("RoomType not found in LandscapeByRoomTypePrefabDict: " + roomType);
            return null; // Return null or handle the error in an appropriate way
        }
    }
    public void DisplayMonsterPrefabHierarchy()
    {
        foreach (var kvp in MonsterPrefabDict)
        {
            Debug.Log("Monster Type: " + kvp.Key);

            foreach (var prefab in kvp.Value)
            {
                Debug.Log("Prefab Name: " + prefab.name);
            }
        }
    }

    private const string m_attackIconPath = "Sprite/UI/Skill/Icon_Attack_";
    private const string m_abilityIconPath = "Sprite/UI/Skill/Icon_Ability_";
    
    public Sprite GetSkillSlotAttackIcon(Player.CharType charType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(m_attackIconPath);
        sb.Append(charType.ToString());
        
        Sprite attackSprite = Resources.Load<Sprite>(sb.ToString());

        return attackSprite;
    }
    
    public Sprite GetSkillSlotAbilityIcon(Player.CharType charType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(m_abilityIconPath);
        sb.Append(charType.ToString());
        
        Sprite abilityIcon = Resources.Load<Sprite>(sb.ToString());

        return abilityIcon;
    }
}