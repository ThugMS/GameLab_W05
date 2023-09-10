using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    #region PublicVariables

    public Sprite fullHeart, threeQuarterHeart, halfHeart, quarterHeart, emptyHeart;
    private Image heartImage;

    #endregion

    #region PrivateVariables
    private HeartStatus currentStatus;
    #endregion

    #region PublicMethod

    public void SetHeartImage(HeartStatus status)
    {
        currentStatus = status;
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Quarter:
                heartImage.sprite = quarterHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.ThreeQuarter:
                heartImage.sprite = threeQuarterHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
    
    public HeartStatus GetHeartStatus()
    {
        return currentStatus;
    }
    
    #endregion

    #region PrivateMethod

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }
    
    #endregion
}

public enum HeartStatus
{
    Empty = 0,
    Quarter = 1,
    Half = 2,
    ThreeQuarter = 3,
    Full = 4
}