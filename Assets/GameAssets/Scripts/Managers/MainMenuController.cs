using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public bool isLevelStarted = false;
    public bool isLevelEnded = false;
    [Header("UI Elements")]
    public GameObject GameplayUI;
    public GameObject MainMenuUI;
    public GameObject LevelSelectionUI;

    [Header("References")]
    public LevelEnd levelEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableGamePlayUI ()
    {
        GameplayUI.SetActive(true);
    }

    public void DisableGamePlayUi()
    {
        GameplayUI.SetActive(false);
    }

    public void EnableMainMenuUI ()
    {
        MainMenuUI.SetActive(true);
    }

    public void DisableMainMenuUI ()
    {
        MainMenuUI.SetActive(false);
    }

    public void EnableLevelUI ()
    {
        LevelSelectionUI.SetActive(true);
    }

    public void DisableLevelUI ()
    {
        LevelSelectionUI.SetActive(false);
    }

    public void ShowLevelUI ()
    {
       Invoke(nameof(ActivateLevelUI) , .25f);
    }

    public void HideLevelUI ()
    {
        Invoke(nameof(DeactivateLevelUI) , .25f);
    }

    void ActivateLevelUI ()
    {
        EnableLevelUI();
        DisableMainMenuUI();
    }

    void DeactivateLevelUI ()
    {
        DisableLevelUI();
        EnableMainMenuUI();
    }


    public void Startlevel ()
    {
        Invoke(nameof(triggerStart) , .25f);
    }
    void triggerStart ()
    {
        if (CommandCenter.Instance.livesManager_.GetCurrentLives() <= 0)
        {
            Debug.LogWarning("No lives left to start the level.");
            return;
        }
        DisableLevelUI();
        EnableGamePlayUI();
        isLevelStarted = true;
        isLevelEnded = false;
        CommandCenter.Instance.levelManager_.ActivateLevel();
        CommandCenter.Instance.gamePlayManager_.SetUpBtns();
        CommandCenter.Instance.timeManager_.StartTimer();
    }
}
