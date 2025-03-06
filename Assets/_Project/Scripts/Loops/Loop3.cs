using UnityEngine;

public class Loop3 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 3;

    public bool IsComplete()
    {
        return true;
    }
}
