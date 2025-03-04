using UnityEngine;

public class Loop2 : MonoBehaviour, ILoop
{
    public bool catEventCompleted = false;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}