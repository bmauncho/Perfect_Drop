using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GifImporter;

public class LevelWonUI : MonoBehaviour
{
    public TMP_Text timetaken;
    public Position [] Positions;
    public Gif [] wonGifs;
    public GifPlayer gifPlayer;

    public void SetWinDetails (string timeTaken)
    {
        timetaken.text = timeTaken;
        if (gifPlayer != null && wonGifs.Length > 0)
        {
            gifPlayer.Gif = wonGifs [Random.Range(0 , wonGifs.Length)];
        }
        else
        {
            Debug.LogWarning("Gif Player or Won Gifs are not set.");
        }
    }
}
