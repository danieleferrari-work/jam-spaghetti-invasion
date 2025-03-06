using UnityEngine;

public class Loop9 : MonoBehaviour, ILoop
{
    public bool gondolierEventCompleted = false; // TODO settare a true quando evento completato

    public int GetLoopNumber() => 9;
    
    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}
