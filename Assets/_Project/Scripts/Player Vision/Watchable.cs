using UnityEngine;
using UnityEngine.Events;

public class Watchable : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textCurrentWatchtime;
    [SerializeField] TMPro.TMP_Text textOverallWatchtime;
    [SerializeField] Color defaultColor;
    [SerializeField] Color watchingColor;

    float currentWatchtime = 0;
    float overallWatchtime = 0;

    public float CurrentWatchtime => currentWatchtime;
    public float OverallWatchtime => overallWatchtime;
    public UnityAction OnBeginWatch;


    void Awake()
    {
        textCurrentWatchtime.color = defaultColor;
        textOverallWatchtime.color = defaultColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            textCurrentWatchtime.color = watchingColor;
            textOverallWatchtime.color = watchingColor;
            OnBeginWatch?.Invoke();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            currentWatchtime += Time.deltaTime;
            overallWatchtime += Time.deltaTime;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            currentWatchtime = 0;
            textCurrentWatchtime.color = defaultColor;
            textOverallWatchtime.color = defaultColor;
        }
    }

    void Update()
    {
        textCurrentWatchtime.text = currentWatchtime.ToString("0.00");
        textOverallWatchtime.text = overallWatchtime.ToString("0.00");
    }
}
