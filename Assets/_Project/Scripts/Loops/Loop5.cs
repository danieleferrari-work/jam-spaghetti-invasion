using UnityEngine;

public class Loop5 : MonoBehaviour, ILoop
{
    public bool facelessEventCompleted = false; // TODO settare a true quando evento completo

    public int GetLoopNumber() => 5;

    public bool IsComplete()
    {
        return facelessEventCompleted;
    }
}
