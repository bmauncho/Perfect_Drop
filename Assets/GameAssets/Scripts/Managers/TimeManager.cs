using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private bool shouldCount = true;
    [SerializeField] private float timeValue = 0;
    [SerializeField] private TMP_Text LevelTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        if (!shouldCount) { return; }

        timeValue += Time.deltaTime;
        DisplayTime(timeValue);
    }

    private void DisplayTime ( float timeToDisplay )
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        LevelTimer.text = string.Format("{0:00} : {1:00}" , minutes , seconds);
    }

    public void AccelerateTimer ( float Time )
    {
        timeValue = timeValue + Time;
    }

    public void DecelerateTimer ( float Time )
    {
        timeValue = timeValue - Time;
    }

    public void StartTimer ()
    {
        shouldCount = true;
    }

    public void stopTimer ()
    {
        shouldCount = false;
    }

    public void ResetTimer ()
    {
        timeValue = 0;
        DisplayTime(timeValue);
    }

    public float GetTimeValue ()
    {
        return timeValue;
    }
}

