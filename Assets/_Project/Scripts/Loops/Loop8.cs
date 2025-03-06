using UnityEngine;

public class Loop8 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 8;

    public bool IsComplete()
    {
        return true;
    }
}
