using UnityEngine;

public class Loop2 : MonoBehaviour, ILoop
{
    [Header("Cat Event")]

    [Tooltip("How many seconds need to pass from the start before the cat starts to meows")]
    public float catMeowStartDelay;

    [Tooltip("Delay between meows")]
    public float catMeowDelay;

    [Tooltip("How many times the cat meows")]
    public float catMeowsCount;

    public string catMeowClipName;

    [Header("Gondolier Event")]

    [Tooltip("WIP How many seconds need to pass from the start before the gondolier starts to sing")]
    public float gondolierSingStartDelay; // TODO usare

    [Tooltip("WIP Delay between singing")]
    public float gondolierSingDelay; // TODO usare

    [Tooltip("WIP How many times the gondolier sing")]
    public float gondolierSingCount; // TODO usare

    public string gondolierSingClipName; // TODO spostare direttamente nell'audio source dell'evento


    public bool gondolierEventCompleted = false;

    public int GetLoopNumber() => 2;

    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}