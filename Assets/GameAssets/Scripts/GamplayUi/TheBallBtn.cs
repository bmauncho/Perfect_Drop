using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheBallBtn : MonoBehaviour
{
    public BallType ballType_;
    public int ballsCount_;
    public TMP_Text remainingballs;
    public Image ballIcon;
    
    public void SetBallType ( BallType ballType,int ballCount,Sprite Icon )
    {
        ballType_ = ballType;
        ballsCount_ = ballCount;
        remainingballs.text = ballCount.ToString();
        ballIcon.sprite = Icon;
    }

    public void DropBall ()
    {
        if (ballsCount_ > 0)
        {
            CommandCenter.Instance.gamePlayManager_.ReduceBalls(ballType_);
            LevelDetails levelDetails = CommandCenter.Instance.levelManager_.ActiveLevel.GetComponent<LevelDetails>();
            levelDetails.DropBall(ballType_);
        }
        ballsCount_--;
        if (ballsCount_ <= 0)
        {
            ballsCount_ = 0;
        }
    }
}
