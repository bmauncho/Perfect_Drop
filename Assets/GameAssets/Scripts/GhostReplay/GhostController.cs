using UnityEditorInternal;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private bool isGhostReplayEnabled = false;
    BallManager ballManager;
    PoolManager poolManager;
    LevelDetails levelDetails;
    LevelGhostManager levelGhostManager;
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
        if (!CommandCenter.Instance) { return; }
        timer = CommandCenter.Instance.timeManager_.GetTimeValue();
        if (levelDetails == null || levelGhostManager == null)
        {
            Debug.Log("No LevelDetails or LevelGhostManager found!");
            return;
        }

        isGhostReplayEnabled = levelGhostManager.HasSavedRecord();
        if (!isGhostReplayEnabled) { return; }

        foreach (var ghost in levelGhostManager.ghosts)
        {
            if(ghost == null)
            {
                Debug.Log("no ghosts available");
                continue; // Skip null ghosts
            }

            foreach(var info in levelGhostManager.ghostIdentifiers)
            {
                if(timer >= ghost.timeStamp 
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
