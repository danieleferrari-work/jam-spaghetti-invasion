using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GondolaAutoPilot : MonoBehaviour
{
    Gondola gondola;
    GondolaMovementManager gondolaMovementManager;
    GondolaAutoPilotTrigger trigger;

    void Awake()
    {
        gondola = GetComponentInParent<Gondola>();
        gondolaMovementManager = gondola.gameObject.GetComponentInChildren<GondolaMovementManager>();
    }

    public void GoTo(GondolaAutoPilotTrigger trigger)
    {
        this.trigger = trigger;

        StartCoroutine(DoMovement());
    }

    private IEnumerator DoMovement()
    {
        float movementDuration = CalculateMovementDuration(trigger.FinalPosition);
        gondola.transform.DOMove(trigger.FinalPosition, movementDuration);
        gondola.transform.DORotate(trigger.FinalRotation, movementDuration);

        yield return new WaitForSeconds(movementDuration + trigger.WaitingTime);

        gondolaMovementManager.DisableAutoPilot();
    }

    private float CalculateMovementDuration(Vector3 targetPosition)
    {
        return Mathf.Abs(Vector3.Distance(gondola.transform.position, targetPosition)) * .5f;
    }
}
