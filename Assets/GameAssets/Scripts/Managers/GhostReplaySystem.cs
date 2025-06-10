using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GhostReplaySystem : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
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
}
