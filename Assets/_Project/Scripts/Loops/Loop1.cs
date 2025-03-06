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

    public bool IsComplete()
    {
        Debug.Log("loop1 completed");
        return true;
    }
}
