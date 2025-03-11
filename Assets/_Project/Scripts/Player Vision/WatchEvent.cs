using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WatchEvent : MonoBehaviour
{
    [SerializeField] EventStartTypes startType;
    [Tooltip("Set false if you want to restart event on end")]
    [SerializeField] bool oneTimeEvent = true;

    [Tooltip("Seconds needed to complete the event")]
    [SerializeField] float timeToWatch = 10;

    [Tooltip("Seconds before the event fails")]
    [SerializeField] float duration = 600;

    [Tooltip("Used only if startType is Delayed")]
    [SerializeField] float startDelay = 2;

    [SerializeField] bool destroyGameObjectOnSuccess = true;
    [SerializeField] bool destroyGameObjectOnFailure = true;


    public UnityAction OnEventStarted;
    public UnityAction OnEventSuccessed;
    public UnityAction OnEventFailed;
    public UnityAction OnStartWatching;
    public UnityAction OnStopWatching;


    Coroutine timer;
    Watchable watchable;

    void Awake()
    {
        watchable = GetComponentInChildren<Watchable>();
    }

    void Start()
    {
        ResetEvent();

        watchable.OnStartWatching += () => { OnStartWatching?.Invoke(); };
        watchable.OnStopWatching += () => { OnStopWatching?.Invoke(); };
    }

    public void ResetEvent()
    {
        if (timer != null)
            StopCoroutine(timer);

        watchable.Reset();

        switch (startType)
        {
            case EventStartTypes.Immediate:
                StartEvent();
                break;
            case EventStartTypes.FirstWatch:
                watchable.OnStartWatching += StartEvent;
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
            watchable.OnStartWatching -= StartEvent;

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

        if (oneTimeEvent)
            this.enabled = false;
        else
            ResetEvent();
    }

    private void CompleteEventWithFailure()
    {
        Debug.Log($"Event {gameObject.name} completed with FAILURE", gameObject);

        OnEventFailed?.Invoke();

        if (destroyGameObjectOnFailure)
            Destroy(gameObject);

        if (oneTimeEvent)
            this.enabled = false;
        else
            ResetEvent();
    }

    public enum EventStartTypes
    {
        FirstWatch,
        Immediate,
        Delayed
    }
}