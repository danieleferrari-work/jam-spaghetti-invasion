using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WatchEvent : MonoBehaviour
{
    [SerializeField] Watchable watchable;
    [SerializeField] EventStartTypes startType;
    [SerializeField] float timeToWatch;
    [SerializeField] float duration;
    [Tooltip("Used only if startType is Delayed")]
    [SerializeField] float startDelay;
    [SerializeField] bool destroyGameObjectOnSuccess;
    [SerializeField] bool destroyGameObjectOnFailure;


    public UnityAction OnEventStarted;
    public UnityAction OnEventSuccessed;
    public UnityAction OnEventFailed;


    Coroutine timer;

    void Start()
    {
        switch (startType)
        {
            case EventStartTypes.Immediate:
                StartEvent();
                break;
            case EventStartTypes.FirstWatch:
                watchable.OnBeginWatch += StartEvent;
                break;
            case EventStartTypes.Delayed:
                StartCoroutine(StartEventDelayed());
                break;
        }
    }

    void Update()
    {
        if (watchable.OverallWatchtime >= timeToWatch)
            CompleteEventWithSuccess();
    }

    public void StartEvent()
    {
        Debug.Log($"Event {gameObject.name} STARTED", gameObject);

        if (startType == EventStartTypes.FirstWatch)
            watchable.OnBeginWatch -= StartEvent;

        timer = StartCoroutine(TimerCoroutine());
        OnEventStarted?.Invoke();
    }

    private IEnumerator StartEventDelayed()
    {
        yield return new WaitForSeconds(startDelay);
        StartEvent();
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(duration);
        CompleteEventWithFailure();
    }

    private void CompleteEventWithSuccess()
    {
        Debug.Log($"Event {gameObject.name} completed with SUCCESS", gameObject);

        OnEventSuccessed?.Invoke();

        if (timer != null)
            StopCoroutine(timer);

        if (destroyGameObjectOnSuccess)
            Destroy(gameObject);

        this.enabled = false;
    }

    private void CompleteEventWithFailure()
    {
        Debug.Log($"Event {gameObject.name} completed with FAILURE", gameObject);

        OnEventFailed?.Invoke();

        if (destroyGameObjectOnFailure)
            Destroy(gameObject);

        this.enabled = false;
    }

    public enum EventStartTypes
    {
        FirstWatch,
        Immediate,
        Delayed
    }
}