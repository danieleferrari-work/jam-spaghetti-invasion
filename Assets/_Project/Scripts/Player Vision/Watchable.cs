using UnityEngine;
using UnityEngine.Events;

public class Watchable : MonoBehaviour
{
    [Tooltip("How far the player needs to be to start watching")]
    [SerializeField] float minDistance = float.MaxValue;

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
    public UnityAction OnBeginWatch;


    void Awake()
    {
#if !UNITY_EDITOR
        textCurrentWatchtime.gameObject.SetActive(false);
        textOverallWatchtime.gameObject.SetActive(false);
        Destroy(GetComponent<AlwaysLookAtCamera>());
#endif
        ChangeTextsColor(defaultColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            if (Vector3.Distance(other.transform.position, transform.position) < minDistance)
            {
                BeginWatch();
            }
            else
            {
                ChangeTextsColor(tooFarColor);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            if (Vector3.Distance(other.transform.position, transform.position) < minDistance)
            {
                if (!alreadyBegin)
                    BeginWatch();

                currentWatchtime += Time.deltaTime;
                overallWatchtime += Time.deltaTime;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            currentWatchtime = 0;
            ChangeTextsColor(defaultColor);
        }
    }

    void BeginWatch()
    {
        alreadyBegin = true;
        ChangeTextsColor(watchingColor);
        OnBeginWatch?.Invoke();
    }

    void Update()
    {
#if UNITY_EDITOR
        textCurrentWatchtime.text = currentWatchtime.ToString("0.00");
        textOverallWatchtime.text = overallWatchtime.ToString("0.00");
#endif
    }

    void ChangeTextsColor(Color color)
    {
#if UNITY_EDITOR
        textCurrentWatchtime.color = color;
        textOverallWatchtime.color = color;
#endif
    }
}
