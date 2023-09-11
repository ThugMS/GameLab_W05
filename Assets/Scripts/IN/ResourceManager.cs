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

    public List <Dictionary<MonsterType, List<GameObject>>> MonsterPrefabDict { get; private set; }


    public Dictionary<RoomType, List<GameObject>> LandscapeByRoomTypePrefabDict { get; private set; }
    public Dictionary<(RoomType, Direction), GameObject> DoorPrefabDict { get; private set; }

    public GameObject m_meleeBossPrefab;
    public GameObject m_rangedBossPrefab;
    public GameObject m_hoverBossPrefab;

    public List<GameObject> m_jamPrefabs;

    public GameObject m_HpPotionPrefab;
    
    public void Init()
    {
        InitMonster();
        InitLandscapeInRoom();
        InitDoor();
    }

    void InitMonster()
    {
        MonsterPrefabDict = new List<Dictionary<MonsterType, List<GameObject>>>();

        int cnt = Enum.GetNames(typeof(MonsterType)).Length;
        for (int stageLevel = 0; stageLevel < 3; stageLevel++)
        {
            Dictionary<MonsterType, List<GameObject>> stageCollection = new Dictionary<MonsterType, List<GameObject>>(); 
            for (int index = 0; index < cnt; index++)
            {
                var type = (MonsterType)index;
                var objList = Resources.LoadAll("Prefabs/Monsters/" + stageLevel.ToString() + "/" + type.ToString(), typeof(GameObject))
                    .Where(obj => obj is GameObject && ((GameObject)obj).tag == "Monster")
                    .Select(obj => (GameObject)obj)
                    .ToList();

                if (objList.Count > 0)
                {
                    print(objList[0].name);
                    stageCollection.Add(type, objList);
                }
            }
            MonsterPrefabDict.Add(stageCollection);
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

    void InitDoor()
    {
        DoorPrefabDict = new();
        var dict = new Dictionary<string, GameObject>();

        foreach (var obj in Resources.LoadAll<GameObject>("Prefabs/Door/"))
        {
            if (!dict.ContainsKey(obj.name))
            {
                dict.Add(obj.name, obj);
            }
        }
        
        for (int rt = 0, cnt = Enum.GetNames(typeof(RoomType)).Length; rt < cnt; rt++)
        {
            for(int direc = 1; direc <= 4; direc++)
            {
                var key = $"Door_{(RoomType)rt}_{(Direction)direc}";
                if (dict.ContainsKey(key))
                {
                    DoorPrefabDict.Add(((RoomType)rt, (Direction)direc), dict[key]);
                }
            }
        }
    }
    


    private const string m_attackIconPath = "Sprite/UI/Skill/Icon_Attack_";
    private const string m_abilityIconPath = "Sprite/UI/Skill/Icon_Ability_";
    private const string m_classIconPath = "Sprite/UI/Profile/PlayerClassImage_";
    
    public Sprite GetSkillSlotAttackIcon(PlayerClassType playerClassType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(m_attackIconPath);
        sb.Append(playerClassType.ToString());
        
        Sprite attackSprite = Resources.Load<Sprite>(sb.ToString());

        return attackSprite;
    }
    
    public Sprite GetSkillSlotAbilityIcon(PlayerClassType playerClassType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(m_abilityIconPath);
        sb.Append(playerClassType.ToString());
        
        Sprite abilityIcon = Resources.Load<Sprite>(sb.ToString());

        return abilityIcon;
    }

    public Sprite GetPlayerClassProfileIcon(PlayerClassType playerClassType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(m_classIconPath);
        sb.Append(playerClassType.ToString());
        
        Sprite abilityIcon = Resources.Load<Sprite>(sb.ToString());

        return abilityIcon;
    }
    
    public Sprite GetGemImage(GemType gemType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Sprite/Item/Gem/Gem_");
        sb.Append(gemType.ToString());
        
        Sprite gemSprite = Resources.Load<Sprite>(sb.ToString());

        return gemSprite;
    }

    public Sprite GetGemIconImage(GemType gemType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Sprite/Item/Gem/GemIcon_");
        sb.Append(gemType.ToString());
        
        Sprite gemSprite = Resources.Load<Sprite>(sb.ToString());

        return gemSprite;
    }

    public GameObject GetBossByType(MonsterType monsterType)
    {
        return monsterType switch
        {
            MonsterType.melee => m_meleeBossPrefab,
            MonsterType.ranged => m_rangedBossPrefab,
            MonsterType.hover => m_hoverBossPrefab,
        };
    }
}