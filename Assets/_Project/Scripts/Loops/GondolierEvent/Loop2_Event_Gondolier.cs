using UnityEngine;

public class Loop2_Event_Gondolier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource3D gondolierSingAudioSource;
    [SerializeField] WatchEvent watchEvent;

    Loop2 loop;

    void Awake()
    {
        loop = GetComponentInParent<Loop2>();

        if (loop.gondolierEventCompleted)
        {
            Destroy(gameObject);
        }

        watchEvent.OnEventStarted += StartSinging;
        watchEvent.OnEventSuccessed += StopSinging;
    }

    private void Start()
    {
        StartSinging();
    }

    void StartSinging()
    {
        gondolierSingAudioSource.Play();
    }

    void StopSinging()
    {
        gondolierSingAudioSource.Stop();
        loop.gondolierEventCompleted = true;
        Destroy(gameObject);
    }
}