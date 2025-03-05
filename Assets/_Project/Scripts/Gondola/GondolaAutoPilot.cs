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
        float movementDuration = CalculateMovementDuration();
        gondola.transform.DOMove(trigger.FinalPosition, movementDuration);
        gondola.transform.DORotate(trigger.FinalRotation, movementDuration);

        yield return new WaitForSeconds(trigger.WaitingTime);

        gondolaMovementManager.DisableAutoPilot();
    }

    private float CalculateMovementDuration()
    {
        return 3;
    }
}
