using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public class BallInfo
{
    public BallType ballType;
    public Ball Model;
}
public class Balls : MonoBehaviour
{
    [Header("Ball Settings")]
    public BallType ActiveballType_;
    public bool isTouchingTrigger = false;
    public Transform model;
    public BallInfo [] ballInfo_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BallInfo GetBallInfo ( BallType ballType )
    {
        for (int j = 0 ; j < ballInfo_.Length ; j++)
        {
            if (ballInfo_ [j].ballType == ballType)
            {
                return ballInfo_ [j];
            }
        }
        return null;
    }

    public void SetActiveBall ( BallType ballType , Vector3 scale = default )
    {

        ActiveballType_ = ballType;

        if(scale == default)
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
