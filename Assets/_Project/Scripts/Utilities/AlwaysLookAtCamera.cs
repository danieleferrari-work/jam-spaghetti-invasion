using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("Missing Main Camera in scene");
            this.enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = mainCamera.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rotation;
    }
}
