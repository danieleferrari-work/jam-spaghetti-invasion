using UnityEngine;

public class Loop3 : MonoBehaviour, ILoop
{
    [Header("Cat Event")]

    [Tooltip("Seconds of pause between cat jumps")]
    [SerializeField] public float catJumpPause = 2;

    [Tooltip("How many jumps the cat does before stops")]
    [SerializeField] public float catJumpRepetitions = 3;


    public bool catEventCompleted = false;

    public int GetLoopNumber() => 3;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}
