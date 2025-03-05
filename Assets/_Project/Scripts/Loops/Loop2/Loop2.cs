using UnityEngine;

public class Loop2 : MonoBehaviour, ILoop
{
    [SerializeField] public float catJumpPause = 2;
    [SerializeField] public float catJumpRepetitions = 3;

    public bool catEventCompleted = false;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}