using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GhostInfo
{
    public BallType BallType;
    public Ghost ghost;
    public float timeStamp;
    public string Identifier = string.Empty;
}

[CreateAssetMenu]
public class LevelGhostManager : ScriptableObject
{
    public int levelNo;
    public bool HasRecord = false;
    public bool isRecording;
    public bool isReplaying;
    public List<GhostInfo> ghosts = new List<GhostInfo>();
    public List<GhostInfo> prevghostIdentifiers = new List<GhostInfo>();
    public List<GhostInfo> ghostIdentifiers = new List<GhostInfo>();

    private void OnEnable ()
    {
        ghostIdentifiers.Clear();
        clearPrevGhostIds();
        prevghostIdentifiers = new List<GhostInfo>(GhostSystemDataSaver.LoadGhostData());
    }

    public void ResetManager ()
    {
        ghosts.Clear();
    }

    public void clearPrevGhostIds ()
    {
        prevghostIdentifiers.Clear();
        Debug.Log("Previous ghost identifiers cleared.");
    }

    public void Record ()
    {
        Debug.Log("Record");
        isRecording = true;
        foreach (var ghostInfo in ghosts)
        {
            if (!ghostInfo.ghost.isRecord)
            {
                ghostInfo.ghost.ResetData();
                ghostInfo.ghost.isRecord = true;
            }
        }
    }

    public void Replay ()
    {
        if (HasSavedRecord())
        {
            Debug.Log("Replay");
            isReplaying = true;
            foreach (var ghostInfo in ghosts)
            {
                if (!ghostInfo.ghost.isReplay)
                {
                    ghostInfo.ghost.isReplay = true;
                }
            }
        }
    }

    public void SetDropTime ( GhostInfo info)
    {
        // Find the corresponding GhostInfo
        Ghost targetGhost = info.ghost;

        foreach (var ghostInfo in ghosts)
        {
            if (ghostInfo.ghost == targetGhost)
            {
                ghostInfo.Identifier = info.Identifier;
                ghostInfo.timeStamp = info.timeStamp;
                ghostIdentifiers.Add(info);
                break;
            }
        }
    }

    public bool HasSavedRecord ()
    {
        bool savedRecord = false;
        int count = 0;
        for(int i = 0 ; i < ghosts.Count ; i++)
        {
            if (ghosts [i].ghost.isPrevDataAvailable())
            {
                count++;
            }
        }
        if (count >= ghosts.Count-1)
        {
            savedRecord = true;
        }

        HasRecord = savedRecord;
        return HasRecord;
    }

    public void ResetRecordingCamera ()
    {
        isRecording = false;
        isReplaying = false;
        foreach (var ghostInfo in ghosts)
        {
            ghostInfo.ghost.ResetData();
            ghostInfo.ghost.isRecord = false;
            ghostInfo.ghost.isReplay = false;
            ghostInfo.timeStamp = 0f;
        }
        HasRecord = HasSavedRecord();
        ghostIdentifiers.Clear();
    }

}
