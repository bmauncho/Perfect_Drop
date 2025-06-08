using UnityEngine;
public enum BallType
{
   Default,
   BasketBall,
   SoccerBall,
   TennisBall,
   GolfBall,
   Baseball,
   RugbyBall,
   VolleyBall,
}

[System.Serializable]
public class BallDetails
{
    public BallType ballType;
    public Sprite ballIcon;
    public float Ballweight = 1f; // Default weight, can be adjusted per ball type
    public Vector3 ballscale = Vector3.one;
    public PhysicsMaterial ballMaterial;
}
public class BallManager : MonoBehaviour
{
    [SerializeField]private BallDetails [] ballDetails;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BallDetails GetBallDetails( BallType ballType )
    {
        for (int j = 0 ; j < ballDetails.Length ; j++)
        {
            if (ballDetails [j].ballType == ballType)
            {
                return ballDetails [j];
            }
        }
        return null;
    }
}
