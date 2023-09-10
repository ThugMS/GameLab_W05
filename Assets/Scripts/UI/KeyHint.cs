using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class KeyHint : MonoBehaviour
{
   #region PrivateVariables
   [SerializeField] private GameObject m_keyBoardImagePrefab;
   [SerializeField] private GameObject m_gamePadImagePrefab;
   private GameObject m_keyBoardImage;
   private GameObject m_gamePadImage;
   
   private bool m_isGamePad;
   #endregion
   
   #region PublicMethod
   
   private void Awake()
   {
      m_keyBoardImage = Instantiate(m_keyBoardImagePrefab, transform);
      m_gamePadImage = Instantiate(m_gamePadImagePrefab, transform);
      
      m_keyBoardImage.SetActive(true);
      m_gamePadImage.SetActive(false);
   }
   
   public void OnControlsChanged(PlayerInput input)
   {
      if (input == null || m_keyBoardImage == null )
         return;
      m_isGamePad = input.currentControlScheme.Equals("Gamepad");
      m_keyBoardImage.SetActive(!m_isGamePad);
      m_gamePadImage.SetActive(m_isGamePad);
   }
   #endregion
}