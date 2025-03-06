using UnityEngine;

public class Loop4 : MonoBehaviour, ILoop
{
    public int GetLoopNumber() => 4;

    public bool IsComplete()
    {
        return true;
    }
}
