using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostInfo
{
    public BallType BallType;
    public Ghost ghost;
}

[CreateAssetMenu]
public class LevelGhostManager : ScriptableObject
{
    public int levelNo;
    public bool isRecording;
    public bool isReplaying;
    public List<GhostInfo> ghosts = new List<GhostInfo>();

    public void ResetManager ()
    {
        ghosts.Clear();
    }
}
