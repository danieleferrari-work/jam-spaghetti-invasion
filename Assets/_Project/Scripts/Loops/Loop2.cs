using UnityEngine;

public class Loop2 : MonoBehaviour, ILoop
{
    [SerializeField] public float catJumpPause = 2;
    [SerializeField] public float catJumpRepetitions = 3;

    public bool catEventCompleted = false;

    public int GetLoopNumber() => 2;
    
    public bool IsComplete()
    {
        Debug.Log($"Loop2 is completed? {catEventCompleted}");
        return catEventCompleted;
    }
}