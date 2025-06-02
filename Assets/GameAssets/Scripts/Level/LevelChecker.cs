using UnityEngine;

public class LevelChecker : MonoBehaviour
{
    float stayTimer = 0f;
    float stayDuration = 3f; // Duration to stay in trigger to count as touching
    void OnTriggerStay ( Collider other )
    {
        // check if any ball is touching the trigger
        if (other.GetComponentInParent<Balls>())
        {
            if(other.GetComponentInParent<Balls>().isTouchingTrigger) { return;}
            //Debug.Log("Ball touching trigger: " + other.name);
            BallType ballType = other.GetComponentInParent<Balls>().ActiveballType_;
            LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
            if (levelDetails == null)
            {
                Debug.LogWarning("LevelDetails component not found on the active level.");
                return;
            }
            stayTimer += Time.deltaTime;
            if (stayTimer >= stayDuration)
            {
                other.GetComponentInParent<Balls>().isTouchingTrigger = true;
                Debug.Log("Ball has stayed in trigger for " + stayDuration + " seconds.");
                resetTimer();
            }
        }
    }

    public void resetTimer ()
    {
        stayTimer = 0f; // Reset the timer when needed
    }
}
