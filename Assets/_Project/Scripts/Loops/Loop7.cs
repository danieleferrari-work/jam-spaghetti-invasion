using UnityEngine;

public class Loop7 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 7;
    
    public bool IsComplete()
    {
        return true;
    }
}
