using UnityEngine;

public class Loop1 : MonoBehaviour, ILoop
{
    public bool facelessEventCompleted = false; // TODO settare a true quando evento completato

    public int GetLoopNumber() => 1;

    public bool IsComplete()
    {
        return facelessEventCompleted;
    }
}
