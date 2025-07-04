using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public LevelWonUI winLevelUI; 
    public LevelFailedUI failedLevelUI; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWinLevelUI ()
    {
        if (winLevelUI != null)
        {
            winLevelUI.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win Level UI is not assigned.");
        }
    }

    public void HideWinLevelUI ()
    {
        if (winLevelUI != null)
        {
            winLevelUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Win Level UI is not assigned.");
        }
    }

    public void ShowFailedLevelUI ()
    {
        if (failedLevelUI != null)
        {
            failedLevelUI.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed Level UI is not assigned.");
        }
    }

    public void HideFailedLevelUI ()
    {
        if (failedLevelUI != null)
        {
            failedLevelUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Failed Level UI is not assigned.");
        }
    }

    public void RetryLevel ()
    {
        HideFailedLevelUI();
        HideWinLevelUI();
        CommandCenter.Instance.ghostReplaySystem_.ResetlevelRecorder();
        CommandCenter.Instance.levelManager_.RetryLevel();
        CommandCenter.Instance.gamePlayManager_.ResetGameplay();
        CommandCenter.Instance.timeManager_.StartTimer();
        CommandCenter.Instance.mainMenuController_.isLevelStarted = true;
        CommandCenter.Instance.mainMenuController_.isLevelEnded = false;

    }

    public void NextLevel ()
    {
        HideFailedLevelUI();
        HideWinLevelUI();
        CommandCenter.Instance.ghostReplaySystem_.ResetlevelRecorder();
        CommandCenter.Instance.levelManager_.EndOfLevel();
        CommandCenter.Instance.gamePlayManager_.ResetGameplay();
        CommandCenter.Instance.timeManager_.StartTimer();
        CommandCenter.Instance.mainMenuController_.isLevelStarted = true;
        CommandCenter.Instance.mainMenuController_.isLevelEnded = false;
    }

    public void MainMenu ()
    {
        HideFailedLevelUI();
        HideWinLevelUI();
        CommandCenter.Instance.ghostReplaySystem_.ResetlevelRecorder();
        CommandCenter.Instance.mainMenuController_.EnableMainMenuUI();
        CommandCenter.Instance.mainMenuController_.DisableGamePlayUi();
        CommandCenter.Instance.levelManager_.DeactivateLevel();
        CommandCenter.Instance.gamePlayManager_.ResetGameplay();
        CommandCenter.Instance.levelManager_.RestartLevel();
        CommandCenter.Instance.mainMenuController_.isLevelStarted = true;
        CommandCenter.Instance.mainMenuController_.isLevelEnded =false;
    }
}
