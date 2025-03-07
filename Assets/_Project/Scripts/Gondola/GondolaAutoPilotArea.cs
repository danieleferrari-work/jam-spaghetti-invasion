using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GondolaAutoPilotArea : MonoBehaviour
{
    [SerializeField] GameObject finalGondolaPosition;
    [SerializeField] bool lockCameraToTarget;
    [SerializeField] GameObject lookTargetPosition;

    [Tooltip("How many seconds need to pass after the gondola reached finalGondolaPosition")]
    [SerializeField] float waitingTime;

    [Tooltip("How many seconds need to pass after a collision to re-enable collision detection")]
    [SerializeField] float resetTime = 30;

    public Vector3 FinalPosition => new Vector3(finalGondolaPosition.transform.position.x, Gondola.BaseHeight, finalGondolaPosition.transform.position.z);
    public Vector3 FinalRotation => finalGondolaPosition.transform.rotation.eulerAngles;
    public Transform LookTarget => lockCameraToTarget ? lookTargetPosition.transform : null;

    public UnityAction OnEndMoving;
    public UnityAction OnStartMoving;

    public static UnityAction<GondolaAutoPilotArea> OnEnableAutoPilot;
    public static UnityAction OnDisableAutoPilot;

    bool disabled = false;

    Gondola gondola;

    void Awake()
    {
        gondola = FindObjectOfType<Gondola>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (disabled)
            return;

        if (other.GetComponent<Gondola>())
        {
            EnableAutoPilot();

            OnStartMoving?.Invoke();

            disabled = true;

            StartCoroutine(ReActivateTrigger());
        }
    }

    IEnumerator ReActivateTrigger()
    {
        yield return new WaitForSeconds(resetTime);
        disabled = false;
    }

    void EnableAutoPilot()
    {
        OnEnableAutoPilot?.Invoke(this);

        StartCoroutine(DoMovement());
    }

    IEnumerator DoMovement()
    {
        float movementDuration = CalculateMovementDuration(FinalPosition);
        gondola.transform.DOMove(FinalPosition, movementDuration);
        gondola.transform.DORotate(FinalRotation, movementDuration);

        yield return new WaitForSeconds(movementDuration);

        OnEndMoving?.Invoke();

        yield return new WaitForSeconds(waitingTime);

        OnDisableAutoPilot?.Invoke();
    }

    float CalculateMovementDuration(Vector3 targetPosition)
    {
        return Mathf.Abs(Vector3.Distance(gondola.transform.position, targetPosition)) * .5f;
    }
}
