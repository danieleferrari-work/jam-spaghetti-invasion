using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop6 : MonoBehaviour, ILoop
{
    public bool catEventCompleted;

    public bool IsComplete()
    {
        return catEventCompleted;
    }
}
