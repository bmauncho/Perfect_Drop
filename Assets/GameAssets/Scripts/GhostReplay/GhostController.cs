using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private bool isGhostReplayEnabled = false;
    [SerializeField] BallManager ballManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] LevelDetails levelDetails;
    [SerializeField] LevelGhostManager levelGhostManager;
    [SerializeField] private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelDetails = GetComponentInParent<LevelDetails>();
        levelGhostManager = levelDetails.levelGhostManager;
        poolManager = CommandCenter.Instance.poolManager_;
        ballManager = CommandCenter.Instance.ballManager_;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CommandCenter.Instance) 
        { 
            Debug.LogWarning("CommandCenter instance not found!");
            return;
        }
        timer = CommandCenter.Instance.timeManager_.GetTimeValue();
        if (levelDetails == null || levelGhostManager == null)
        {
            Debug.Log("No LevelDetails or LevelGhostManager found!");
            return;
        }

        isGhostReplayEnabled = levelGhostManager.HasSavedRecord();
        if (!isGhostReplayEnabled) 
        {
            Debug.Log("Ghost replay is not enabled or no ghosts available to replay!");
            return;
        }

        if (levelGhostManager.ghosts == null || levelGhostManager.ghosts.Count <= 0)
        {
            Debug.Log("No ghosts available to replay!");
            return; // No ghosts to replay
        }
        foreach (var ghost in levelGhostManager.ghosts)
        {
            if(ghost == null)
            {
                Debug.Log("no ghosts available");
                continue; // Skip null ghosts
            }

            foreach(var info in levelGhostManager.prevghostIdentifiers)
            {
                //Debug.Log($"Checking ghost: {ghost.ghost.name}\n, timeCheck: {timer >= ghost.timeStamp}\n, " +
                //          $"objectMatch: {ghost.ghost == info.ghost}\n, identifierMatch: {ghost.Identifier == info.Identifier}\n, " +
                //          $"isSpawned: {!ghost.ghost.isSpawned}\n, hasPrevData: {ghost.ghost.isPrevDataAvailable()}\n");

                if (timer >= info.timeStamp 
                    && ghost.ghost == info.ghost
                    && ghost.Identifier == info.Identifier
                    && !ghost.ghost.isSpawned
                    && ghost.ghost.isPrevDataAvailable())
                {
                    Debug.Log("show ghost ball!");   
                    GameObject ghostBall = poolManager.GetFromPool(PoolType.Ghosts , levelDetails.GetSpawnPoint().position , Quaternion.identity , transform);
                    GhostBall gb = ghostBall.GetComponent<GhostBall>();
                    ghostBall.GetComponent<GhostPlayer>().ghost = ghost.ghost;
                    Vector3 scale = ballManager.GetBallDetails(info.BallType).ballscale;
                    float width = scale.x;
                    ghostBall.GetComponentInChildren<TrailRenderer>().startWidth = Mathf.Clamp01(width);
                    gb.SetActiveBall(info.BallType,scale);
                    gb.gameObject.SetActive(true);
                    ghost.ghost.isSpawned = true; // Mark the ghost as spawned
                    ghost.ghost.isReplay = true;
                    break;
                }
            }
        }
    }
}
