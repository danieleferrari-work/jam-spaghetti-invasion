using UnityEngine;

public class LoopExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var looper = other.GetComponentInChildren<Looper>();

        if (looper)
        {
            LoopsManager.instance.NextLoop();
        }
    }
}
