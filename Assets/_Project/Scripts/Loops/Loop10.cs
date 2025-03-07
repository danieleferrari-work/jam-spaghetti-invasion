using UnityEngine;

public class Loop10 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 10;

    public bool gondolierEventCompleted = false;
    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}
