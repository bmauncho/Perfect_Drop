using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    public static CommandCenter Instance { get; private set; }
    public LevelManager levelManager_;
    public GamePlayManager gamePlayManager_;
    public PoolManager poolManager_;
    public BallManager ballManager_;
    public CurrencyManager currencyManager_;
    public LivesManager livesManager_;
    public TimeManager timeManager_;
    public MainMenuController mainMenuController_;
    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensures only one instance exists
            return;
        }

        Instance = this;
    }
}
