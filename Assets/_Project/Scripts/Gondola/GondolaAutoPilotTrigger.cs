using UnityEngine;

public class GondolaAutoPilotTrigger : MonoBehaviour
{
    [SerializeField] GameObject finalGondolaPosition;
    [SerializeField] float waitingTime;

    public Vector3 FinalPosition => finalGondolaPosition.transform.position;
    public Vector3 FinalRotation => finalGondolaPosition.transform.rotation.eulerAngles;
    public float WaitingTime => waitingTime;

    
    void OnTriggerEnter(Collider other)
    {
        var gondola = other.GetComponent<Gondola>();

        if (gondola)
        {
            var gondolaMovementManager = gondola.GetComponentInChildren<GondolaMovementManager>();
            gondolaMovementManager.EnableAutoPilot(this);
        }
    }
}
