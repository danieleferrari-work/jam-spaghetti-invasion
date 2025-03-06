using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop9 : MonoBehaviour, ILoop
{
    public bool gondolierEventCompleted;

    public int GetLoopNumber() => 9;
    
    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}
