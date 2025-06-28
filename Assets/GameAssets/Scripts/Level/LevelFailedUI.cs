using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GifImporter;

public class LevelFailedUI : MonoBehaviour
{
    public TMP_Text LevelFailTxt;
    public TMP_Text LevelFailReasonTxt;
    public GameObject AddTimeBtn;
    public GameObject RetryBtn;
    public Gif [] failGifs;
    public GifPlayer gifPlayer;
    
    public void SetFailDetails (
        string LevelfailText,
        string LevelFailReasonText )
    {

        LevelFailTxt.text = LevelfailText;
        LevelFailReasonTxt.text = LevelFailReasonText;
        if (gifPlayer != null && failGifs.Length > 0)
        {
            gifPlayer.Gif = failGifs [Random.Range(0 , failGifs.Length)];
        }
        else
        {
            Debug.LogWarning("Gif Player or Fail Gifs are not set.");
        }
    }
}
