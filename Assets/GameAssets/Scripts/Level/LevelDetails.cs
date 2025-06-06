using UnityEngine;
[System.Serializable]
public class LevelInfo
{
    public BallType ballType;
    public int ballCount;
}
public class LevelDetails : MonoBehaviour
{
    PoolManager poolManager;
    [Header("Level Details")]
    [SerializeField] private bool isBallTouchingTrigger;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LevelInfo[] levelInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolManager = CommandCenter.Instance.poolManager_;
    }

    public void DropBall( BallType ballType)
    {
        GameObject ball = poolManager.GetFromPool(PoolType.Balls , spawnPoint.position , Quaternion.identity , transform);
        BallManager ballManager = CommandCenter.Instance.ballManager_;

        Vector3 scale = ballManager.GetBallDetails(ballType).ballscale;
        float width = scale.x;
        ball.GetComponentInChildren<TrailRenderer>().startWidth = Mathf.Clamp01(width);
        ball.GetComponent<Balls>().SetActiveBall(ballType,scale);
        Rigidbody rb = ball.GetComponentInChildren<Rigidbody>();
        rb.linearVelocity = Vector3.down * 5f; // Makes it fall fast
        //Debug.Log($"Drop {ballType}");
    }
    public LevelInfo[] GetLevelInfo ()
    {
        return levelInfo;
    }

    public void LevelSucceded ()
    {
        Debug.Log("Level Succeeded!");
    }

    public void LevelFailed ()
    {
        Debug.Log("Level Failed!");
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
}
