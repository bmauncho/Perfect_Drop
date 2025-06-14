using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GhostReplaySystem : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    public LevelGhostManager activelevel;
    private HashSet<int> existingLevelNos = new HashSet<int>();
    public List<LevelGhostManager> replaylevelManagers = new List<LevelGhostManager>();

    [ContextMenu("Create replayManagers")]
    public void CreateReplayManager () 
    { 
        int totalLevels = levelManager.Levels.Count;
        for (int i = 0 ; i < totalLevels ; i++)
        {
            int level = i + 1;
            if (managerExists(level))
                continue;

            LevelGhostManager levelGhostManager = ScriptableObject.CreateInstance<LevelGhostManager>();
            levelGhostManager.levelNo = level;
            replaylevelManagers.Add(levelGhostManager);

#if UNITY_EDITOR
            string folderPath = "Assets/Resources/LevelReplayData";
            string levelFolder = $"{folderPath}/Level_{level}";
            AssetDatabase.CreateFolder(folderPath , $"Level_{level}");

            string assetPath = $"{levelFolder}/LevelGhostManager_{level}.asset";
            AssetDatabase.CreateAsset(levelGhostManager , assetPath);
            //for eachball create a ghost for each ball.
            var ghostInfos = ghostInfoDatas(i);

            for (int j = 0 ; j < ghostInfos.Count ; j++)
            {
                BallType type = ghostInfos [j].type;
                for (int k = 0 ; k < ghostInfos [j].balls ; k++)
                {
                    Ghost ghost = ScriptableObject.CreateInstance<Ghost>();
                    string ghostAssetPath = $"{levelFolder}/Ghost_{type}_{level}_{k}.asset";
                    AssetDatabase.CreateAsset(ghost , ghostAssetPath);

                    GhostInfo ghostInfo = new GhostInfo
                    {
                        BallType = type ,
                        ghost = ghost ,
                    };
                    ghostInfo.ghost.recordFrequency = 240f;
                    levelGhostManager.ghosts.Add(ghostInfo);
                    // Possibly store or associate ghostInfo with levelGhostManager or other tracking
                }
            }
#endif
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    [ContextMenu("Reset replayManagers")]
    public void ResetManagers ()
    {
        replaylevelManagers.Clear();
#if UNITY_EDITOR
        string folderPath = "Assets/Resources/LevelReplayData";
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            // Delete LevelGhostManager assets
            string [] ghostManagerPaths = AssetDatabase.FindAssets("t:LevelGhostManager" , new [] { folderPath })
                .Select(AssetDatabase.GUIDToAssetPath).ToArray();

            foreach (string path in ghostManagerPaths)
            {
                AssetDatabase.DeleteAsset(path);
            }

            // Delete Ghost assets
            string [] ghostPaths = AssetDatabase.FindAssets("t:Ghost" , new [] { folderPath })
                .Select(AssetDatabase.GUIDToAssetPath).ToArray();

            foreach (string path in ghostPaths)
            {
                AssetDatabase.DeleteAsset(path);
            }

            // Optionally, delete empty level folders
            var subFolders = AssetDatabase.GetSubFolders(folderPath);
            foreach (var folder in subFolders)
            {
                if (!AssetDatabase.FindAssets("" , new [] { folder }).Any())
                {
                    AssetDatabase.DeleteAsset(folder);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    public void AssignLevelReplayManagers ()
    {
        int currentLevel = CommandCenter.Instance.levelManager_.CurrentLevelCounter;
        LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
        for (int i = 0;i<replaylevelManagers.Count;i++)
        {
            if(currentLevel == i)
            {
                LevelGhostManager ghostManager = Resources.Load<LevelGhostManager>($"LevelReplayData/level_{i+1}/LevelGhostManager_{i+1}");
                levelDetails.levelGhostManager = ghostManager;
            }
        }

        SetActiveLevel(levelDetails);
    }

    private bool managerExists ( int level )
    {
        return existingLevelNos.Contains(level);
    }

    private List<GameObject> Levels ()
    {
        List<GameObject> levelslist = new List<GameObject>();
        for(int i = 0 ;i<levelManager.Levels.Count ; i++)
        {
            string levelName = "TestLevel";
            GameObject level = Resources.Load<GameObject>($"Levels/{levelName}");
            levelslist.Add( level );
        }
      
        return levelslist;
    }

    //return ball type + no of balls
    public List<(BallType type,int balls)> ghostInfoDatas (int level)
    {
        List<(BallType type, int balls)> tempData = new List<(BallType type, int balls)> ();
        LevelInfo [] levelinfo = Levels() [level].GetComponent<LevelDetails>().GetLevelInfos();
        for (int j = 0 ; j < levelinfo.Length ; j++)
        {
            tempData.Add((levelinfo [j].ballType, levelinfo [j].ballCount));
        }
        return tempData;
    }

    public void SetActiveLevel (LevelDetails level)
    {
        activelevel = level.GetComponent<LevelDetails>().levelGhostManager;
    }

    public void ResetlevelRecorder ()
    {
        activelevel.ResetRecordingCamera();
        foreach (var ghostInfo in activelevel.ghosts)
        {
            if (ghostInfo.ghost != null)
            {
                ghostInfo.ghost.isRecord = false;
                ghostInfo.ghost.isReplay = false;
                ghostInfo.ghost.isSpawned = false;
            }
        }
    }

    public void resetGhosts ()
    {
        foreach (var levelGhostManager in replaylevelManagers)
        {
            if (levelGhostManager != null 
                && levelGhostManager.ghosts != null 
                && levelGhostManager.ghosts.Count > 0)
            {
                foreach (var ghostInfo in levelGhostManager.ghosts)
                {
                    if (ghostInfo.ghost != null)
                    {
                        ghostInfo.ghost.isRecord = false;
                        ghostInfo.ghost.isReplay = false;
                        ghostInfo.ghost.isSpawned = false;
                    }
                }
            }
        }
    }
}
