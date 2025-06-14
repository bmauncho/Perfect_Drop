using UnityEngine;

public class GhostController : MonoBehaviour
{
    PoolManager poolManager;
    LevelDetails levelDetails;
    LevelGhostManager levelGhostManager;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelDetails = GetComponentInParent<LevelDetails>();
        levelGhostManager = levelDetails.levelGhostManager;
        poolManager = CommandCenter.Instance.poolManager_;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.unscaledDeltaTime;
        if (!CommandCenter.Instance) { return; }
        if (levelDetails == null || levelGhostManager == null)
        {
            return;
        }
        if (!levelGhostManager.HasSavedRecord()) { return; }

        foreach (var ball in levelDetails.balls)
        {
            foreach(var ghostId in levelGhostManager.ghostIdentifiers)
            {
                Balls balls = ghostId.balls;
                if (balls.Identifier == ball.Identifier)
                {
                    if(ghostId.timeStamp >= timer)
                    {
                        // Show ghost ball
                        GameObject ghostBall = poolManager.GetFromPool(PoolType.Ghosts , levelDetails.GetSpawnPoint().position , Quaternion.identity , transform);
                        GhostBall gb = ghostBall.GetComponent<GhostBall>();
                        ghostBall.GetComponent<GhostPlayer>().ghost = ball.GetComponent<GhostRecorder>().ghost;
                        gb.ActiveballType_ = ball.ActiveballType_;
                        gb.gameObject.SetActive(true);
                    }
                }
            }
        }

    }
}
