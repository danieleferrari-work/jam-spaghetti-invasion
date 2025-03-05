using UnityEngine;

public class Loop3_Event_Cat : MonoBehaviour, ILoop
{
    public bool catEventCompleted = false;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}
