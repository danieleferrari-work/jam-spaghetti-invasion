using UnityEngine;

public class Loop1 : MonoBehaviour, ILoop
{
    [Header("Gondolier Event")]

    [Tooltip("How many seconds need to pass from the start before the gondolier starts to sing")]
    public float gondolierSingStartDelay;

    [Tooltip("Delay between singing")]
    public float gondolierSingDelay;

    [Tooltip("How many times the gondolier sing")]
    public float gondolierSingCount;

    public string gondolierSingClipName;


    public bool facelessEventCompleted = false; // TODO settare a true quando evento completato

    public int GetLoopNumber() => 1;

    public bool IsComplete()
    {
        return facelessEventCompleted;
    }
}
