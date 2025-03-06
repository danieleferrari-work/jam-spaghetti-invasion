using UnityEngine;

public class Loop5 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 5;

    public bool IsComplete()
    {
        return true;
    }
}
