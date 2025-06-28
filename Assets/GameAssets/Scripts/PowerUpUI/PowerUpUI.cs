using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image powerUpImage;
    public TMP_Text powerUpInfoTitle;
    public TMP_Text powerUpInfo;
    [Header("Button Elements")]
    public bool isAdBtn =false;
    public GameObject adButton;
    public GameObject powerUpButton;

    public void SetUpPowerUPBTn (bool isAdBtn,Sprite Icon, string Title,string Description)
    {
        adButton.SetActive(isAdBtn);
        powerUpButton.SetActive(!isAdBtn);
        powerUpImage.sprite = Icon;
        powerUpInfoTitle.text = Title;
        powerUpInfo.text = Description;
    }
}
