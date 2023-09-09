using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHint : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Image keyImage;
    [SerializeField] private GameObject keyArrowImagePrefab, keyAImagePrefab, keySImagePrefab;
    [SerializeField] private GameObject padArrowImagePrefab, padAImagePrefab, padSImagePrefab;

    private Dictionary<ActionType, GameObject> keyboardActionPrefabDict;
    private Dictionary<ActionType, GameObject> padActionPrefabDict;

    #endregion

    private void Awake()
    {
        keyboardActionPrefabDict = new Dictionary<ActionType, GameObject>
        {
            { ActionType.Move, keyArrowImagePrefab },
            { ActionType.Attack, keyAImagePrefab },
            { ActionType.Ability, keySImagePrefab },
        };

        padActionPrefabDict = new Dictionary<ActionType, GameObject>
        {
            { ActionType.Move, padArrowImagePrefab },
            { ActionType.Attack, padAImagePrefab },
            { ActionType.Ability, padSImagePrefab },
        };
    }
    
    #region PublicMethod
    public void UpdateKeyImage(ActionType actionType, InputType inputType)
    {
        if (keyImage.transform.childCount > 0)
        {
            Destroy(keyImage.transform.GetChild(0).gameObject);
        }

        GameObject newPrefab = inputType == InputType.Keyboard ? 
            keyboardActionPrefabDict[actionType] : 
            padActionPrefabDict[actionType];

        Instantiate(newPrefab, keyImage.transform);
    }
    
    #endregion

    #region PrivateMethod
   
    #endregion
}

public enum ActionType
{
    Move,
    Attack,
    Ability,
}

public enum InputType
{
    Keyboard,
    Pad,
}