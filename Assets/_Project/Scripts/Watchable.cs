using UnityEngine;

public class Watchable : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textWatchtime;
    [SerializeField] Color defaultColor;
    [SerializeField] Color watchingColor;

    float watchtime = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            textWatchtime.color = watchingColor;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            watchtime += Time.deltaTime;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerVisionManager>())
        {
            watchtime = 0;
            textWatchtime.color = defaultColor;
        }
    }

    void Update()
    {
        textWatchtime.text = watchtime.ToString("0.00");
    }
}
