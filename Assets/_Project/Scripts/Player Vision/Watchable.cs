using UnityEngine;
using UnityEngine.Events;

public class Watchable : MonoBehaviour
{
    [Tooltip("How far the player needs to be to start watching")]
    [SerializeField] float minDistance = float.MaxValue;
    [SerializeField] bool watchOnlyWhileZooming = true;

    [Header("Debug")]
    [SerializeField] TMPro.TMP_Text textCurrentWatchtime;
    [SerializeField] TMPro.TMP_Text textOverallWatchtime;
    [SerializeField] Color defaultColor;
    [SerializeField] Color tooFarColor;
    [SerializeField] Color watchingColor;


    float currentWatchtime = 0;
    float overallWatchtime = 0;
    bool alreadyBegin = false;

    public float CurrentWatchtime => currentWatchtime;
    public float OverallWatchtime => overallWatchtime;

    public UnityAction OnStartWatching;
    public UnityAction OnStopWatching;


    void Awake()
    {
#if !UNITY_EDITOR
        textCurrentWatchtime.gameObject.SetActive(false);
        textOverallWatchtime.gameObject.SetActive(false);
        Destroy(GetComponent<AlwaysLookAtCamera>());
        ChangeTextsColor(defaultColor);
#endif
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
        else
        {
            ChangeTextsColor(tooFarColor);
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

        ChangeTextsColor(watchingColor);
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

        ChangeTextsColor(watchingColor);
    }

    void StopWatch()
    {
        alreadyBegin = false;
        currentWatchtime = 0;
        OnStopWatching?.Invoke();

        ChangeTextsColor(defaultColor);
    }

    void Update()
    {
#if UNITY_EDITOR
        textCurrentWatchtime.text = currentWatchtime.ToString("0.00");
        textOverallWatchtime.text = overallWatchtime.ToString("0.00");
#endif

        if (watchOnlyWhileZooming && !PlayerCameraManager.instance.Zooming)
            StopWatch();
    }

    void ChangeTextsColor(Color color)
    {
#if UNITY_EDITOR
        textCurrentWatchtime.color = color;
        textOverallWatchtime.color = color;
#endif
    }
}
