using UnityEngine;
[System.Serializable]
public class LevelInfo
{
    public BallType ballType;
    public int ballCount;
    public int OriginalballCount;
}
public class LevelDetails : MonoBehaviour
{
    PoolManager poolManager;
    [Header("Level Details")]
    [SerializeField] private bool isBallTouchingTrigger;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LevelInfo[] levelInfo;
    public bool isLevelWon =false;
    public bool isLevelFailed = false;
    public LevelGhostManager levelGhostManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolManager = CommandCenter.Instance.poolManager_;
        CommandCenter.Instance.ghostReplaySystem_.AssignLevelReplayManagers();
        SetUp();
    }
    private void SetUp ()
    {
        foreach (var level in levelInfo)
        {
            if (level != null)
            {
                level.OriginalballCount = level.ballCount;
            }
        }
    }

    public void DropBall( BallType ballType)
    {
        GameObject ball = poolManager.GetFromPool(PoolType.Balls , spawnPoint.position , Quaternion.identity , transform);
        BallManager ballManager = CommandCenter.Instance.ballManager_;

        Vector3 scale = ballManager.GetBallDetails(ballType).ballscale;
        float width = scale.x;
        ball.GetComponentInChildren<TrailRenderer>().startWidth = Mathf.Clamp01(width);
        ball.GetComponent<Balls>().SetActiveBall(ballType,scale);
        MeshCollider meshCollider = ball.GetComponentInChildren<MeshCollider>();
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.mass = ballManager.GetBallDetails(ballType).Ballweight; // Set the weight of the ball
        meshCollider.material = ballManager.GetBallDetails(ballType).ballMaterial; // Set the physics material
        CommandCenter.Instance.gamePlayManager_.AddBall(ball); // Add the ball to the gameplay manager
        ball.GetComponent<Balls>().isFalling = true; // Set the ball to falling state
        AssignGhost(ball , ballType);
    }

    public void AssignGhost (GameObject Ball, BallType type = BallType.BasketBall)
    {
        int currLevel = CommandCenter.Instance.levelManager_.CurrentLevelCounter+1;
        int ballindex = GetGhostIndex(type);
        string fileAssetpath = $"LevelReplayData/Level_{currLevel}/Ghost_{type}_{currLevel}_{ballindex}";
        Debug.Log(fileAssetpath);
        Ghost ghost = Resources.Load<Ghost>(fileAssetpath);
        Ball.GetComponent<GhostRecorder>().ghost = ghost;
    }
    public int GetGhostIndex ( BallType type )
    {
        //o-original
        //c-current
        // ((o-c)-1)
        int index = 0;
        int currLevel = 0;
        int origLevel = 0;
        for( int i = 0;i<levelInfo.Length;i++)
        {
            if (levelInfo [i].ballType == type)
            {
                currLevel = levelInfo [i].ballCount;
                origLevel = levelInfo [i].OriginalballCount;
                break;
            }
        }
        index = ((origLevel-currLevel)-1);
        return index;
    }
    public LevelInfo[] GetLevelInfo ()
    {
        return levelInfo;
    }

    public void LevelSucceded ()
    {
        Debug.Log("Level Succeeded!");
        isLevelWon = true;
        CommandCenter.Instance.mainMenuController_.levelEnd.ShowWinLevelUI(); // Show the win level UI
        CommandCenter.Instance.timeManager_.stopTimer();
    }

    public void LevelFailed ()
    {
        Debug.Log("Level Failed!");
        isLevelFailed = true;
        CommandCenter.Instance.mainMenuController_.levelEnd.ShowFailedLevelUI(); // Show the failed level UI
        CommandCenter.Instance.timeManager_.stopTimer();
    }

    public bool IsBallTouchingTrigger ()
    {
        return isBallTouchingTrigger;
    }


    public bool CheckForBallsTouchingTrigger ()
    {
        Balls [] balls = GetComponentsInChildren<Balls>();

        if (balls.Length == 0)
        {
            return false;
        }
        int ballsTouchingTriggerCount = 0;
        foreach (var ball in balls)
        {
            if (ball.isTouchingTrigger)
            {
                ballsTouchingTriggerCount++;
            }
        }

        if (ballsTouchingTriggerCount > 0)
        {
            isBallTouchingTrigger = true;
        }
        else
        {
            isBallTouchingTrigger = false;
        }

        return isBallTouchingTrigger;
    }

    public int remainingBalls ()
    {
        int remainingBallsCount = 0;
        foreach (var info in levelInfo)
        {
            if (info.ballCount > 0)
            {
                remainingBallsCount++;
            }
        }

        if (remainingBallsCount <= 0)
        {
            return 0;
        }
        else
        {
            return remainingBallsCount;
        }
    }

    public LevelInfo [] GetLevelInfos ()
    {
        return levelInfo;
    }

    public void resetLevel ()
    {
        isBallTouchingTrigger = false;
        isLevelFailed = false;
        isLevelWon = false;
    }
}
