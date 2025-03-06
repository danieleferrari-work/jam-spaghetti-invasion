using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop9 : MonoBehaviour, ILoop
{
    public bool gondolierEventCompleted;

    public bool IsComplete()
    {
        return gondolierEventCompleted;
    }
}
