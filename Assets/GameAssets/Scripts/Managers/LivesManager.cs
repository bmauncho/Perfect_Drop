using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    [Header("Lives Settings")]
    [SerializeField] private int currentLives = 3;
    public const int maxLives = 5;
    [Header("Lives UI")]
    [SerializeField] private bool shouldCount = true;
    [SerializeField] private float timeValue = 90;
    [SerializeField] private float DefaultTimeValue = 10;
    [SerializeField] private RectTransform livesTimePanel;
    [SerializeField] private TMP_Text LivesTimer;
    [SerializeField] private GameObject [] lives;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshLivesUI();
    }

    // Update is called once per frame
    void Update()
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

    public void StartTimer ()
    {
       StartCoroutine(showLivesTimer());
    }

    public void stopTimer ()
    {
        shouldCount = false;
    }

    IEnumerator showLivesTimer()
    {
        Tween myTween = livesTimePanel.DOAnchorPosY(0 , .25f).SetEase(Ease.InOutQuad);
        yield return myTween.WaitForCompletion();
        shouldCount = true;
        yield return null;
    }

    IEnumerator HideLivesTimePanel ()
    {
        Tween myTween = livesTimePanel.DOAnchorPosY(60 , .25f).SetEase(Ease.InOutQuad);
        yield return myTween.WaitForCompletion();
        shouldCount = false;
        yield return null;
    }

    private void DisplayTime ( float timeToDisplay )
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;

            // increase life
            AddLives(1);
            // reset timer
            ResetCountDownTimer();
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        LivesTimer.text = string.Format("{0:00} : {1:00}" , minutes , seconds);
    }

    public void ResetCountDownTimer ()
    {
        timeValue = DefaultTimeValue;
        if (currentLives < maxLives)
        {
            shouldCount = true;
        }
        else if(currentLives >= maxLives)
        {
            shouldCount = false;
            StartCoroutine(HideLivesTimePanel());
        }
    }

    public void RefreshLivesUI ()
    {
        for (int i = 0 ; i < lives.Length ; i++)
        {
            if (i < currentLives)
            {
                lives [i].SetActive(true);
            }
            else
            {
                lives [i].SetActive(false);
            }
        }

        if (currentLives >= maxLives)
        {
            StartCoroutine(HideLivesTimePanel());
        }
        else
        {
            StartCoroutine(showLivesTimer());
        }
    }


    public void AddLives ( int amount )
    {
        currentLives += amount;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }
        RefreshLivesUI();
    }

    public void RemoveLives ( int amount )
    {
        currentLives -= amount;
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        RefreshLivesUI();
    }

    public int GetCurrentLives ()
    {
        return currentLives;
    }
}
