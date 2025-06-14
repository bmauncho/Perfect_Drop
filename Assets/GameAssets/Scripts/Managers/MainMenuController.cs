using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public bool isLevelStarted = false;
    public bool isLevelEnded = false;
    [Header("UI Elements")]
    public GameObject GameplayUI;
    public GameObject MainMenuUI;

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
        DisableMainMenuUI();
        EnableGamePlayUI();
        //show levelSelection UI


        isLevelStarted = true;
        isLevelEnded = false;
        CommandCenter.Instance.levelManager_.ActivateLevel();
        CommandCenter.Instance.gamePlayManager_.SetUpBtns();
        CommandCenter.Instance.timeManager_.StartTimer();
        LevelDetails activeLevelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
        CommandCenter.Instance.ghostReplaySystem_.SetActiveLevel(activeLevelDetails);
    }
}
