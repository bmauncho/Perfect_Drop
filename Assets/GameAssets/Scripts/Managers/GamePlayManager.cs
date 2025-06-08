using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private bool canShowEndLevelUI = false;
    public TheBallBtn [] theBallBtns;
    [SerializeField] private List<BallType> OrderofBallTypesBtns = new List<BallType>();
    [SerializeField] private List<GameObject> BallsIncurrentLevel = new List<GameObject>();
    private float Delay = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if (canShowEndLevelUI) { return; } // Prevents multiple calls to end level UI
        if (CommandCenter.Instance)
        {
            if(!CommandCenter.Instance.levelManager_.ActiveLevel){ return; }
            LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
            TimeManager timeManager = CommandCenter.Instance.timeManager_;
            if (levelDetails == null)
            {
                Debug.LogWarning("LevelDetails component not found on the active level.");
                return;
            }
            if (!levelDetails.IsBallTouchingTrigger())
            {
                levelDetails.CheckForBallsTouchingTrigger();

                if (isBallOutOfBounds() && timeManager.GetTimeValue() > 0)
                {
                    //show failed UI
                    if (!canShowEndLevelUI)
                    {
                        canShowEndLevelUI = true;
                        levelDetails.LevelFailed();
                        Delay = 3f; // Reset the delay timer
                    }
                    return; // Exit early if any ball is out of bounds
                }

                if (levelDetails.remainingBalls() <= 0 && timeManager.GetTimeValue()>0)
                {
                    //wait for a few seconds to check if any balls are touching the trigger due to physics
                    Delay -= Time.deltaTime;
                    if (Delay > 0) { return; } // Wait for the delay before checking again
                    // Check if any balls are touching the trigger and time is still left
                    if (levelDetails.CheckForBallsTouchingTrigger())
                    {
                        //show failed UI
                        if (!canShowEndLevelUI)
                        {
                            canShowEndLevelUI = true;
                            levelDetails.LevelFailed();
                            Delay = 3f; // Reset the delay timer
                        }
                        return; // Exit early if balls are touching the trigger
                    }
                    else
                    {
                        // If no balls are touching the trigger and time is left, we consider it a success
                        //show success UI
                        if (!canShowEndLevelUI)
                        {
                            canShowEndLevelUI = true;
                            levelDetails.LevelSucceded();
                            Delay = 3f; // Reset the delay timer
                        }
                    }
                }
                else if (levelDetails.remainingBalls() > 0 && timeManager.GetTimeValue() <= 0)
                {
                    //show failed UI
                    if (!canShowEndLevelUI)
                    {
                        canShowEndLevelUI = true;
                        levelDetails.LevelFailed();
                        Delay = 3f; // Reset the delay timer
                    }
                }
            }
            else
            {
                //show failed UI
                if(!canShowEndLevelUI)
                {
                    canShowEndLevelUI = true;
                    levelDetails.LevelFailed();
                }
            }
        }
    }

    public TheBallBtn [] GetTheBallBtns ()
    {
        return theBallBtns;
    }

    public void SetUpBtns ()
    {
        OrderofBallTypesBtns.Clear();
        LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();

        for (int i = 0 ; i < theBallBtns.Length ; i++)
        {
            if (i < levelDetails.GetLevelInfo().Length)
            {
                theBallBtns [i].gameObject.SetActive(true);
                BallType ballType = levelDetails.GetLevelInfo() [i].ballType;
                int ballCount = levelDetails.GetLevelInfo() [i].ballCount;
                Sprite ballSprite = CommandCenter.Instance.ballManager_.GetBallDetails(ballType).ballIcon;
                // Set the ball type and count for the button
                theBallBtns [i].SetBallType(ballType , ballCount , ballSprite);
                // Add the ball type to the order list
                OrderofBallTypesBtns.Add(ballType);
            }
            else
            {
                // hide extra buttons
                theBallBtns [i].gameObject.SetActive(false);
            }
        }
    }

    public void ReduceBalls( BallType ballType )
    {
        LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
        for (int i = 0 ; i < theBallBtns.Length ; i++)
        {
            if (theBallBtns [i].ballType_ == ballType)
            {
                int remainingCount = int.Parse(theBallBtns [i].remainingballs.text);
                if (remainingCount > 0)
                {
                    remainingCount--;
                    levelDetails.GetLevelInfo() [i].ballCount = remainingCount; // Update the level info
                    theBallBtns [i].remainingballs.text = remainingCount.ToString();
                }
                break;
            }
        }
    }

    public void AddBalls(BallType ballType )
    {
        LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
        for (int i = 0 ; i < theBallBtns.Length ; i++)
        {
            if (theBallBtns [i].ballType_ == ballType)
            {
                int remainingCount = int.Parse(theBallBtns [i].remainingballs.text);
                remainingCount++;
                levelDetails.GetLevelInfo() [i].ballCount = remainingCount; // Update the level info
                theBallBtns [i].remainingballs.text = remainingCount.ToString();
                break;
            }
        }
    }


    public void AddBall(GameObject ball )
    {
        if (ball == null) { return; }
        if (!BallsIncurrentLevel.Contains(ball))
        {
            BallsIncurrentLevel.Add(ball);
        }
    }

    public void ResetBalllsInCurrentLevelList()
    {
        BallsIncurrentLevel.Clear();
    }


    public bool isBallOutOfBounds ()
    {
        if(BallsIncurrentLevel.Count <= 0) { return false; }
        // Check if any ball in the current level is out of bounds
        for(int i = 0 ; i < BallsIncurrentLevel.Count ; i++)
        {
            if (BallsIncurrentLevel [i] == null) { continue; }
            BallOutOfBoundsCheck checker = BallsIncurrentLevel [i].GetComponent<BallOutOfBoundsCheck>();
            if (checker.IsBallOutOfBounds())
            {
                return true;
            }
        }

        return false;
    }
}
