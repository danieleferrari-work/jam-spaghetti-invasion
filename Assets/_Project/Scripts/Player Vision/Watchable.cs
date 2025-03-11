using UnityEngine;
using UnityEngine.Events;

public class Watchable : MonoBehaviour
{
    [Tooltip("How far the player needs to be to start watching")]
    [SerializeField] float minDistance = float.MaxValue;
    [SerializeField] bool watchOnlyWhileZooming = true;


    float currentWatchtime = 0;
    float overallWatchtime = 0;
    bool alreadyBegin = false;

    public float CurrentWatchtime => currentWatchtime;
    public float OverallWatchtime => overallWatchtime;

    public UnityAction OnStartWatching;
    public UnityAction OnStopWatching;


    public void Reset()
    {
        currentWatchtime = 0;
        overallWatchtime = 0;
        alreadyBegin = false;

        OnStartWatching = null;
        OnStopWatching = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerVisionManager>())
            return;

        if (watchOnlyWhileZooming && !PlayerCameraManager.instance.Zooming)
            return;

        if (Vector3.Distance(other.transform.position, transform.position) < minDistance)
        {
            BeginWatch();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<PlayerVisionManager>())
            return;

        if (watchOnlyWhileZooming && !PlayerCameraManager.instance.Zooming)
            return;

        if (Vector3.Distance(other.transform.position, transform.position) > minDistance)
            return;

        if (!alreadyBegin)
            BeginWatch();

        currentWatchtime += Time.deltaTime;
        overallWatchtime += Time.deltaTime;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerVisionManager>())
            return;

        StopWatch();
    }

    void BeginWatch()
    {
        alreadyBegin = true;
        OnStartWatching?.Invoke();
    }

    void StopWatch()
    {
        alreadyBegin = false;
        currentWatchtime = 0;
        OnStopWatching?.Invoke();
    }

    void Update()
    {
        if (watchOnlyWhileZooming && !PlayerCameraManager.instance.Zooming)
            StopWatch();
    }
}
