using UnityEngine;

public class Loop10 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 10;

    public bool gondolierEventCompleted = false;
    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }

    void Start()
    {
        FindObjectOfType<LoopExit>().gameObject.SetActive(false);
        FindObjectOfType<LoopFinalExit>(true).gameObject.SetActive(true);
        FindObjectOfType<Loop10_Event_GondolierV2>().CheckEnding();
    }
}
