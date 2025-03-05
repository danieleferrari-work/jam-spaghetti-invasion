using UnityEngine;
using UnityEngine.Events;

public class Watchable : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textCurrentWatchtime;
    [SerializeField] TMPro.TMP_Text textOverallWatchtime;
    [SerializeField] Color defaultColor;
    [SerializeField] Color tooFarColor;
    [SerializeField] Color watchingColor;
    [SerializeField] float minDistance = float.MaxValue;

    float currentWatchtime = 0;
    float overallWatchtime = 0;
    bool alreadyBegin = false;

    public float CurrentWatchtime => currentWatchtime;
    public float OverallWatchtime => overallWatchtime;
    public UnityAction OnBeginWatch;


    void Awake()
    {
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

    private void BeginWatch()
    {
        alreadyBegin = true;
        ChangeTextsColor(watchingColor);
        OnBeginWatch?.Invoke();
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

    void Update()
    {
        textCurrentWatchtime.text = currentWatchtime.ToString("0.00");
        textOverallWatchtime.text = overallWatchtime.ToString("0.00");
    }

    void ChangeTextsColor(Color color)
    {
        textCurrentWatchtime.color = color;
        textOverallWatchtime.color = color;
    }
}
