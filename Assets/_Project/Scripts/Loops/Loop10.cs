using UnityEngine;

public class Loop10 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 10;
    
    public bool IsComplete()
    {
        return true;
    }
}
