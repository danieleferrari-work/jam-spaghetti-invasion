using UnityEngine;

public class Loop3 : MonoBehaviour, ILoop
{
    public bool catEventCompleted = false;

    public bool IsComplete()
    {
        Debug.Log($"Loop3 is completed? {catEventCompleted}");
        return catEventCompleted;
    }
}
