using UnityEngine;
using UnityEngine.Events;

public class Loop2_Cat : MonoBehaviour
{
    public static UnityAction OnCatJumpedOnBoat;

    void OnTriggerEnter(Collider other)
    {
        var gondola = other.GetComponent<Gondola>();

        if (gondola)
        {
            gondola.catOnBoat.SetActive(true);
            OnCatJumpedOnBoat?.Invoke();
        }
    }
}
