using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public bool isLevelStarted = false;
    public bool isLevelEnded = false;
    [Header("UI Elements")]
    public GameObject GameplayUI;
    public GameObject MainMenuUI;
    public GameObject LevelSelectionUI;
    public GameObject PowerUpSelection;

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
    void ActivatePowerUpUI ()
    {
        PowerUpSelection.SetActive(true);
    }

    void DeactivatePowerUpUI ()
    {
        PowerUpSelection.SetActive(false);
    }

    public void ShowPowerUps ()
    {
        Invoke(nameof(triggerPowerUpSelection) , .25f);
    }

    void triggerPowerUpSelection ()
    {
        DisableLevelUI();
        ActivatePowerUpUI();
    }

    public void Startlevel ()
    {
        Invoke(nameof(triggerStart) , .25f);
    }

    void triggerStart ()
    {
        DeactivatePowerUpUI();
        EnableGamePlayUI();
        DisableMainMenuUI();
        isLevelStarted = true;
        isLevelEnded = false;
        CommandCenter.Instance.levelManager_.ActivateLevel();
        CommandCenter.Instance.gamePlayManager_.SetUpBtns();
        CommandCenter.Instance.timeManager_.StartTimer();
    }
}
