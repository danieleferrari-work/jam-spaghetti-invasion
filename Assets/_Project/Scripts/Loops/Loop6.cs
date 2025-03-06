using UnityEngine;

public class Loop6 : MonoBehaviour, ILoop
{
    public bool catEventCompleted = false;

    public int GetLoopNumber() => 6;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}
