using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GondolaAutoPilotArea : MonoBehaviour
{
    [SerializeField] GameObject finalGondolaPosition;
    
    [Tooltip("How many seconds need to pass after the gondola reached finalGondolaPosition")]
    [SerializeField] float waitingTime;

    [Tooltip("How many seconds need to pass after a collision to re-enable collision detection")]
    [SerializeField] float resetTime = 30;

    public Vector3 FinalPosition => new Vector3(finalGondolaPosition.transform.position.x, Gondola.BaseHeight, finalGondolaPosition.transform.position.z);
    public Vector3 FinalRotation => finalGondolaPosition.transform.rotation.eulerAngles;
    public float WaitingTime => waitingTime;
    public UnityEvent OnFinish;

    bool disabled = false;


    void OnTriggerEnter(Collider other)
    {
        if (disabled)
            return;

        var gondola = other.GetComponent<Gondola>();

        if (gondola)
        {
            disabled = true;
            var gondolaMovementManager = gondola.GetComponentInChildren<GondolaMovementManager>();
            gondolaMovementManager.EnableAutoPilot(this);
            StartCoroutine(ReActivateTrigger());
        }
    }

    IEnumerator ReActivateTrigger()
    {
        yield return new WaitForSeconds(resetTime);
        disabled = false;
    }
}
