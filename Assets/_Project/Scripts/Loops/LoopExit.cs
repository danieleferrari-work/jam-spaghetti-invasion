using UnityEngine;

public class LoopExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var gondola = other.GetComponent<Gondola>();

        if (gondola)
        {
            LoopsManager.instance.OnLoopExit();
        }
    }
}
