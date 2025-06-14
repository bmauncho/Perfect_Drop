using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class GhostInfoListWrapper
{
    public List<GhostInfoSaveData> data;
}

[System.Serializable]
public class GhostInfoSaveData
{
    public BallType BallType;
    public string ghostName;
    public float timeStamp;
    public string Identifier;
}

public static class GhostSystemDataSaver
{
    public static List<GhostInfoSaveData> ConvertToSaveData ( List<GhostInfo> data )
    {
        var saveData = new List<GhostInfoSaveData>();
        foreach (var info in data)
        {
            saveData.Add(new GhostInfoSaveData
            {
                BallType = info.BallType ,
                ghostName = info.ghost != null ? info.ghost.name : string.Empty ,
                timeStamp = info.timeStamp ,
                Identifier = info.Identifier
            });
        }
        return saveData;
    }

    public static void SaveGhostData ( List<GhostInfo> ghostIdentifiers )
    {
        var saveData = ConvertToSaveData(ghostIdentifiers);
        string json = JsonConvert.SerializeObject(new GhostInfoListWrapper { data = saveData } , Formatting.Indented);

        PlayerPrefs.SetString("GhostData" , json);
        PlayerPrefs.Save();
    }

    public static List<GhostInfo> LoadGhostData ()
    {
        string json = PlayerPrefs.GetString("GhostData" , "");
        if (string.IsNullOrEmpty(json)) return new List<GhostInfo>();

        GhostInfoListWrapper wrapper = JsonConvert.DeserializeObject<GhostInfoListWrapper>(json);
        var result = new List<GhostInfo>();

        foreach (var saveData in wrapper.data)
        {
            result.Add(new GhostInfo
            {
                BallType = saveData.BallType ,
                ghost = LoadGhostByName(saveData.ghostName,saveData.BallType) ,
                timeStamp = saveData.timeStamp ,
                Identifier = saveData.Identifier
            });
        }

        return result;
    }

    private static Ghost LoadGhostByName ( string ghostName ,BallType type)
    {
        if (string.IsNullOrEmpty(ghostName)) return null;
        string folderPath = "LevelReplayData"; // Resources.Load uses relative path
        int levelNo = GetLevel(ghostName ,type); 
        return Resources.Load<Ghost>($"{folderPath}/Level_{levelNo}/{ghostName}");
    }

    public static int GetLevel ( string name , BallType ballType )
    {
        int levelNo = 0;

        if (name.Contains($"Ghost_{ballType}_"))
        {
            string [] parts = name.Split('_');
            if (parts.Length > 2 && int.TryParse(parts [2] , out levelNo))
            {
                return levelNo;
            }
        }

        return levelNo;
    }

}
