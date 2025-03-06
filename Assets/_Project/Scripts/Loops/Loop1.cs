using UnityEngine;

public class Loop1 : MonoBehaviour, ILoop
{
    [Tooltip("How many seconds need to pass from the start before the cat starts to meows")]
    public float catMeowStartDelay;

    [Tooltip("Delay between meows")]
    public float catMeowDelay;

    [Tooltip("How many times the cat meows")]
    public float catMeowsCount;

    public string catMeowClipName;

    [Tooltip("How many seconds need to pass from the start before the gondolier starts to sing")]
    public float gondolierSingStartDelay;

    [Tooltip("Delay between singing")]
    public float gondolierSingDelay;

    [Tooltip("How many times the gondolier sing")]
    public float gondolierSingCount;

    public string gondolierSingClipName;
    public int GetLoopNumber() => 1;

    public bool IsComplete()
    {
        Debug.Log("loop1 completed");
        return true;
    }
}
