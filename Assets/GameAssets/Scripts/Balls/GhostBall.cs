using UnityEngine;

public class GhostBall : MonoBehaviour
{
    public BallType ActiveballType_;
    public Transform model;
    public BallInfo [] ballInfo_;

    public void SetActiveBall ( BallType ballType , Vector3 scale = default )
    {

        ActiveballType_ = ballType;

        if (scale == default)
        {
            scale = Vector3.one;
        }

        for (int i = 0 ; i < ballInfo_.Length ; i++)
        {
            if (ballInfo_ [i].ballType == ballType)
            {
                ballInfo_ [i].Model.gameObject.SetActive(true);
                model.transform.localScale = scale;
            }
            else
            {
                ballInfo_ [i].Model.gameObject.SetActive(false);
            }
        }
    }
}
