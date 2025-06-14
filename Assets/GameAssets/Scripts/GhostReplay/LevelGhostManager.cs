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
}

[CreateAssetMenu]
public class LevelGhostManager : ScriptableObject
{
    public int levelNo;
    public bool HasRecord = false;
    public bool isRecording;
    public bool isReplaying;
    float timer;
    public List<GhostInfo> ghosts = new List<GhostInfo>();
    public List<(Balls balls, float timeStamp)> ghostIdentifiers = new List<(Balls balls, float timeStamp)>();
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

    private void Update ()
    {
        timer += Time.unscaledDeltaTime;
    }

    public void SetDropTime ( Balls balls)
    {
        if (!ghostIdentifiers.Exists(g => g.balls == balls))
        {
            ghostIdentifiers.Add((balls, timer));
        }
    }

    public bool HasSavedRecord ()
    {
        bool savedRecord = false;
        
        for(int i = 0 ; i < ghosts.Count ; i++)
        {
            if (ghosts [i].ghost.isPrevDataAvailable())
            {
                savedRecord = true;
                break;
            }
        }
        savedRecord = HasRecord;
        return HasRecord;
    }

    public void ResetRecordingCamera ()
    {
        isRecording = false;
        isReplaying = false;
        timer = 0f;
        HasRecord = false;
        foreach (var ghostInfo in ghosts)
        {
            ghostInfo.ghost.ResetData();
            ghostInfo.ghost.isRecord = false;
            ghostInfo.ghost.isReplay = false;
        }
        ghostIdentifiers.Clear();
    }
}
