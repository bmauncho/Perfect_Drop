using UnityEngine;

public class GhostController : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!CommandCenter.Instance) { return; }
        timer = CommandCenter.Instance.timeManager_.GetTimeValue();
        if (levelDetails == null || levelGhostManager == null)
        {
            return;
        }
        if (!levelGhostManager.HasSavedRecord()) { return; }

        foreach(var ghost in levelGhostManager.ghosts)
        {
            foreach(var info in levelGhostManager.ghostIdentifiers)
            {
                if(timer >= ghost.timeStamp 
                    && ghost.ghost == info.ghost
                    && ghost.Identifier == info.Identifier)
                {
                    GameObject ghostBall = poolManager.GetFromPool(PoolType.Ghosts , levelDetails.GetSpawnPoint().position , Quaternion.identity , transform);
                    GhostBall gb = ghostBall.GetComponent<GhostBall>();
                    ghostBall.GetComponent<GhostPlayer>().ghost = ghost.ghost;
                    gb.ActiveballType_ = info.BallType;
                    gb.gameObject.SetActive(true);
                }
            }
        }
    }
}
