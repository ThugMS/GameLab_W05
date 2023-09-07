using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

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
        }
        else
        {
            Destroy(this);
        }
    }

    public Dictionary<MonsterType, GameObject> MeleeMonsterList { get; private set; } = new();
    public Dictionary<MonsterType, GameObject> RangedMonsterList { get; private set; } = new();
    public Dictionary<MonsterType, GameObject> HoverMonsterList { get; private set; } = new();

    public Dictionary<MonsterType, GameObject> MonsterPrefabDict { get; private set; }

    /// <summary>
    /// ���� ���ҽ����� �ε�
    /// </summary>
    public void Init()
    {
        // Load Monster Prefabs 
        MonsterPrefabDict = new();
        for (int index = 0, cnt = Enum.GetNames(typeof(MonsterType)).Length; index < cnt; index++)
        {
            var type = (MonsterType)index;
            var objList = Resources.LoadAll("Prefabs/Monsters/" + type.ToString(), typeof(GameObject));

            foreach (var obj in objList)
            {
                if (obj as GameObject != null && (obj as GameObject).tag == "Monster")
                {
                    
                        MonsterPrefabDict.Add(type, (obj as GameObject));
                        switch (type)
                        {
                            case MonsterType.melee: MeleeMonsterList.Add(type, obj as GameObject); break;
                            case MonsterType.ranged: MeleeMonsterList.Add(type, obj as GameObject); break;
                            case MonsterType.hover: MeleeMonsterList.Add(type, obj as GameObject); break;
                        }
                }
            }
        }


    }


}