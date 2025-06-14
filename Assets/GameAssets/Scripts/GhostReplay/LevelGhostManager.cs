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
    public List<GhostInfo> ghostIdentifiers = new List<GhostInfo>();
    public void ResetManager ()
    {
        ghosts.Clear();
    }

    public void Record ()
    {
        if (!isRecording)
        {
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
    }

    public void Replay ()
    {
        if (!isReplaying && HasRecord)
        {
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
        if (count > 0)
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
        }
        HasRecord = HasSavedRecord();
        ghostIdentifiers.Clear();
    }
}
