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
            Init();
        }
        else
        {
            Destroy(this);
        }
    }

    public Dictionary<MonsterType, List<GameObject>> MonsterPrefabDict { get; private set; }

    public void Init()
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

}