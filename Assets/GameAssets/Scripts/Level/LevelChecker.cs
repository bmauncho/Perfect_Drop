using System.Collections;
using TMPro;
using UnityEngine;

public class LevelChecker : MonoBehaviour
{
    float stayTimer = 0f;
    float flashDelay = 1.5f; // Delay for before flashing effect
    float stayDuration = 3f; // Duration to stay in trigger to count as touching
    private Renderer rend;
    private Color originalColor;
    private Coroutine flashCoroutine;
    bool isTriggrerActive = false; // Flag to check if the trigger is active
    private void Start ()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color; // Store the original color
            originalColor.a = 0f;
            rend.material.color = originalColor; // Set the initial color to transparent
        }
        else
        {
            Debug.LogWarning("Renderer component not found on LevelChecker.");
        }
    }
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
            if(stayTimer > flashDelay)
            {
                if(!isTriggrerActive)
                {
                    isTriggrerActive = true; // Set the flag to true when the trigger is active
                    TriggerFlash();
                }
              
            }
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

    public void TriggerFlash(float delay = 1f,bool islooping = true )
    {
        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine); // Stop any existing flash coroutine
        }

        flashCoroutine = StartCoroutine(Flash(delay , islooping));
    }

    private IEnumerator Flash ( float delay , bool isLooping )
    {
        Color flashColor = Color.red;
        flashColor.a = 0.98f; // Set target alpha
        float pulseDuration = delay; // Total time for one pulse in and out

        int loopCount = isLooping ? 3 : 1;

        for (int i = 0 ; i < loopCount ; i++)
        {
            // Lerp from original to flash color
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / ( pulseDuration / 2f ); // Half the delay to reach peak
                rend.material.color = Color.Lerp(originalColor , flashColor , t);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f); // Wait before pulsing back
            // Lerp back from flash color to original
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / ( pulseDuration / 2f );
                rend.material.color = Color.Lerp(flashColor , originalColor , t);
                yield return null;
            }
        }

        isTriggrerActive = false;
        flashCoroutine = null;
        stayTimer = 0f; // Reset the timer after flashing
        if (rend != null)
            rend.material.color = originalColor;
        else
            Debug.LogWarning("Renderer component not found on LevelChecker.");
    }


}
