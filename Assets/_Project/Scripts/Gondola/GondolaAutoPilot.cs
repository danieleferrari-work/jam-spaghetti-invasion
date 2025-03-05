using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GondolaAutoPilot : MonoBehaviour
{
    Gondola gondola;
    GondolaMovementManager gondolaMovementManager;
    GondolaAutoPilotArea area;

    void Awake()
    {
        gondola = GetComponentInParent<Gondola>();
        gondolaMovementManager = gondola.gameObject.GetComponentInChildren<GondolaMovementManager>();
    }

    public void GoTo(GondolaAutoPilotArea area)
    {
        this.area = area;

        StartCoroutine(DoMovement());
    }

    private IEnumerator DoMovement()
    {
        float movementDuration = CalculateMovementDuration(area.FinalPosition);
        gondola.transform.DOMove(area.FinalPosition, movementDuration);
        gondola.transform.DORotate(area.FinalRotation, movementDuration);

        yield return new WaitForSeconds(movementDuration + area.WaitingTime);

        gondolaMovementManager.DisableAutoPilot();

        area.OnFinish?.Invoke();
    }

    private float CalculateMovementDuration(Vector3 targetPosition)
    {
        return Mathf.Abs(Vector3.Distance(gondola.transform.position, targetPosition)) * .5f;
    }
}
