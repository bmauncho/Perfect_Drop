using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private bool shouldCount = true;
    [SerializeField] private float timeValue = 90;
    [SerializeField] private float defaultTimeValue = 90;
    [SerializeField] private TMP_Text LevelTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        if (timeValue > 0)
        {
            if (!shouldCount) { return; }

            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }
        DisplayTime(timeValue);
    }

    private void DisplayTime ( float timeToDisplay )
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        LevelTimer.text = string.Format("{0:00} : {1:00}" , minutes , seconds);
    }

    public void IncreaseCountDownTimer ( float Time )
    {
        timeValue = timeValue + Time;
    }

    public void DecreaseCountDownTimer ( float Time )
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
        timeValue = defaultTimeValue;
        DisplayTime(timeValue);
    }

    public float GetTimeValue ()
    {
        return timeValue;
    }
}

