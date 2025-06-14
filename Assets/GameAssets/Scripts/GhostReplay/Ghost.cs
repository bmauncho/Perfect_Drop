using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RecordData
{
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Vector3> rotation;
}

[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    public bool isSpawned;
    public bool isRecord;
    public bool isReplay;
    public float recordFrequency;
    public RecordData prevRecord;
    public RecordData currRecord;
    public void ResetData ()
    {
        if (!isPrevDataAvailable() && !isCurrDataAvailable())
        {
            Debug.Log("No Data to reset.");
            return;
        }

        if (isPrevDataAvailable() && isCurrDataAvailable() 
            || !isPrevDataAvailable() && isCurrDataAvailable())
        {
            Debug.Log("Save currRecord to prevRecord! ");
            ClearPrevData();
            prevRecord = new RecordData
            {
                timeStamp = new List<float>(currRecord.timeStamp) ,
                position = new List<Vector3>(currRecord.position) ,
                rotation = new List<Vector3>(currRecord.rotation)
            };
            Debug.Log("Clear  currRecord! ");
            ClearCurrentData();
        }
    }


    public void ClearPrevData ()
    {
        prevRecord.timeStamp.Clear();
        prevRecord.position.Clear();
        prevRecord.rotation.Clear();
    }

    public void ClearCurrentData ()
    {
        currRecord.timeStamp.Clear();
        currRecord.position.Clear();
        currRecord.rotation.Clear();
    }

    public bool isPrevDataAvailable ()
    {
        bool isRecordAvailable =
            ( prevRecord.timeStamp != null && prevRecord.timeStamp.Count > 0 ) &&
            ( prevRecord.position != null && prevRecord.position.Count > 0 ) &&
            ( prevRecord.rotation != null && prevRecord.rotation.Count > 0 );
        return isRecordAvailable;
    }

    public bool isCurrDataAvailable ()
    {
        bool isRecordAvailable =
            ( currRecord.timeStamp != null && currRecord.timeStamp.Count > 0 ) &&
            ( currRecord.position != null && currRecord.position.Count > 0 ) &&
            ( currRecord.rotation != null && currRecord.rotation.Count > 0 );
        return isRecordAvailable;
    }

    private void OnDisable ()
    {
        isSpawned = false;
        isRecord = false;
        isReplay = false;
    }

    public void resetTriggers ()
    {
        isSpawned = false;
        isRecord = false;
        isReplay = false;
    }
}
