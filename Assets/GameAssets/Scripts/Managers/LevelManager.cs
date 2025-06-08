using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class LevelManager : MonoBehaviour
{
    [Header("Level Details")]
    public Transform LevelParent;
    public GameObject ActiveLevel;

    [Space(10)]
    public int LevelsQueLength = 3;
    public int CurrentLevelCounter = 0;
    public List<string> Levels = new List<string>();
    public List<GameObject> LevelsQue = new List<GameObject>();

    [Header("Debugging")]
    public int DebugLevel;

    void Start ()
    {
        ConfigureLevelsQueue();
    }

    private int GetCurrentLevel ()
    {
        if (!PlayerPrefs.HasKey("LEVEL"))
            PlayerPrefs.SetInt("LEVEL" , 0);

        return PlayerPrefs.GetInt("LEVEL");
    }

    public void ActivateLevel ()
    {
        LevelParent.gameObject.SetActive(true);
        ActiveLevel = GetActiveLevel();
    }

    public void DeactivateLevel ()
    {
        LevelParent.gameObject.SetActive(false);
    }

    public GameObject GetActiveLevel ()
    {
        if (LevelsQue.Count > 0)
        {
            ActiveLevel = LevelsQue [0];
            return ActiveLevel;
        }
        return null;
    }
    private void ConfigureLevelsQueue ()
    {
        CurrentLevelCounter = GetCurrentLevel();

        for (int i = CurrentLevelCounter ; i < CurrentLevelCounter + LevelsQueLength + 1 ; i++)
        {
            int levelIndex = i < Levels.Count ? i : Random.Range(2 , Levels.Count);
            SpawnLevelIntoQueue(levelIndex);
        }

        if (LevelsQue.Count > 0)
            LevelsQue [0].SetActive(true);
    }

    private void SpawnLevelIntoQueue ( int levelIndex )
    {
        if (levelIndex < 0 || levelIndex >= Levels.Count)
        {
            Debug.LogWarning($"Invalid level index: {levelIndex}");
            return;
        }

        string levelName = Levels [levelIndex].Split('=') [0].Trim();
        GameObject levelPrefab = Resources.Load<GameObject>($"Levels/{levelName}");

        if (levelPrefab == null)
        {
            Debug.LogWarning($"Level not found in Resources: {levelName}");
            return;
        }

        GameObject spawnedLevel = Instantiate(levelPrefab , LevelParent);
        spawnedLevel.SetActive(false);
        spawnedLevel.name = levelName;
        LevelsQue.Add(spawnedLevel);
    }

    public void SpawnOneLevel ()
    {
        int nextIndex = CurrentLevelCounter + LevelsQueLength;

        if (nextIndex < Levels.Count)
            SpawnLevelIntoQueue(nextIndex);
        else
            SpawnLevelIntoQueue(Random.Range(2 , Levels.Count));
    }

    public void EndOfLevel ()
    {
        if (LevelsQue.Count < 2) return;

        CurrentLevelCounter++;
        PlayerPrefs.SetInt("LEVEL" , CurrentLevelCounter);

        SpawnOneLevel();

        LevelsQue [1].SetActive(true);
        LevelsQue [0].SetActive(false);

        GameObject oldLevel = LevelsQue [0];
        LevelsQue.RemoveAt(0);
        Destroy(oldLevel);
    }

    public void RetryLevel ()
    {
        ReloadLevelAtIndex(0);
    }

    public void RestartLevel ()
    {
        ReloadLevelAtIndex(0);
    }

    private void ReloadLevelAtIndex ( int index )
    {
        if (index >= LevelsQue.Count) return;

        GameObject oldLevel = LevelsQue [index];
        oldLevel.SetActive(false);

        string levelName = oldLevel.name.Split('=') [0].Trim();
        GameObject levelPrefab = Resources.Load<GameObject>($"Levels/{levelName}");

        if (levelPrefab == null)
        {
            Debug.LogWarning($"Level not found in Resources: {levelName}");
            return;
        }

        GameObject newLevel = Instantiate(levelPrefab , LevelParent);
        newLevel.name = oldLevel.name;
        LevelsQue [index] = newLevel;
        newLevel.SetActive(true);
        Destroy(oldLevel);
        ActiveLevel = newLevel;
    }

    public void EnableFakeLevel ()
    {
        PlayerPrefs.SetInt("LEVEL" , DebugLevel);
    }

    [ContextMenu("Populate Levels")]
    public void PopulateLevels ()
    {
        Levels.Clear();
        for (int i = 0 ; i < 100 ; i++)
        {
            string levelName = $"Level_{i}";
            GameObject level = Resources.Load<GameObject>($"Levels/{levelName}");
            if (level != null)
                Levels.Add(levelName);
        }

#if UNITY_EDITOR
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
    }
}
