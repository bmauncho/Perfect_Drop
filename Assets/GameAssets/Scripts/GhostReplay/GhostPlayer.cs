using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    public Ghost ghost;
    public float timeValue;
    public int index1;
    public int index2;

    private void Awake ()
    {
        timeValue = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(ghost == null) { return; }
        timeValue += Time.unscaledDeltaTime;
        if (ghost.prevRecord.timeStamp.Count == 0 ||
                ghost.prevRecord.timeStamp == null)
        {
            return;
        }

        if (ghost.isReplay)
        {
            GetIndex();
            SetTransform();
        }
    }

    public void GetIndex ()
    {

        for (int i = 0;i< ghost.prevRecord.timeStamp.Count - 2;i++)
        {
            if(ghost.prevRecord.timeStamp[i] == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if(ghost.prevRecord.timeStamp[i] < timeValue && timeValue < ghost.prevRecord.timeStamp [i + 1])
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = ghost.prevRecord.timeStamp.Count - 1;
        index2 = ghost.prevRecord.timeStamp.Count - 1;
    }

    public void SetTransform ()
    {

        if(index1 == index2)
        {
            this.transform.position = ghost.prevRecord.position [index1];
            this.transform.eulerAngles = ghost.prevRecord.rotation [index1];
        }
        else
        {
            float Interpolationfactor = (timeValue-ghost.prevRecord.timeStamp[index1]) / ( ghost.prevRecord.timeStamp [index2] - ghost.prevRecord.timeStamp [index1]);

            this.transform.position = Vector3.Lerp(ghost.prevRecord.position [index1], ghost.prevRecord.position [index2], Interpolationfactor);
            Quaternion rot1 = Quaternion.Euler(ghost.prevRecord.rotation [index1]);
            Quaternion rot2 = Quaternion.Euler(ghost.prevRecord.rotation [index2]);

            Quaternion interpolatedRotation = Quaternion.Slerp(rot1 , rot2 , Interpolationfactor);
            this.transform.eulerAngles = interpolatedRotation.eulerAngles;
        }
    }

}
