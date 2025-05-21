using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    public static CommandCenter Instance { get; private set; }
    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensures only one instance exists
            return;
        }

        Instance = this;
    }
}
