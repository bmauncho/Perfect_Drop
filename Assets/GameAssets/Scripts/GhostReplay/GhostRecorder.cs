using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public Ghost ghost;
    public float timer;
    public float timeValue;

    private void Awake ()
    {
        if (ghost != null && ghost.isRecord)
        {
            ghost.ResetData();
            timeValue = 0;
            timer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!CommandCenter.Instance)return;
        if(ghost==null) return;

        if (CommandCenter.Instance.mainMenuController_.isLevelEnded)
        {
            return;
        }

        timer += Time.unscaledDeltaTime;
        timeValue += Time.unscaledDeltaTime;

        if(ghost.isRecord && timer >= 1 / ghost.recordFrequency)
        {
            ghost.currRecord.timeStamp.Add(timeValue);
            ghost.currRecord.position.Add(this.transform.position);
            ghost.currRecord.rotation.Add(this.transform.eulerAngles);
            timer = 0;
        }
    }
}
