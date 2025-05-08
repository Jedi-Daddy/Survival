using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Camera playerCamera;

    void Start()
    {
        
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("Main camera not found. Please assign the camera in the inspector.");
            }
        }
    }

    void Update()
    {
        if (playerCamera != null)
        {
            Vector3 lookDirection = playerCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-lookDirection);  
        }
    }
}
