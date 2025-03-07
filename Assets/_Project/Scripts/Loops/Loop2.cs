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

    public string gondolierSingClipName;


    public bool gondolierEventCompleted = false; // TODO settare true quando evento gondoliere è completato

    public int GetLoopNumber() => 2;

    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}