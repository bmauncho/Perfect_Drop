using UnityEngine;

public class BallOutOfBoundsCheck : MonoBehaviour
{
    private Camera mainCamera;

    void Start ()
    {
        mainCamera = Camera.main;
    }

    public bool IsBallOutOfBounds ()
    {
        if (mainCamera== null) { return false; } // If no main camera, cannot check bounds
        Vector3 position = transform.position;
        Vector3 screenMin = mainCamera.ViewportToWorldPoint(new Vector3(0 , 0 , position.z - mainCamera.transform.position.z));
        Vector3 screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1 , 1 , position.z - mainCamera.transform.position.z));
        return position.x < screenMin.x || position.x > screenMax.x ||
               position.y < screenMin.y || position.y > screenMax.y;
    }
}
