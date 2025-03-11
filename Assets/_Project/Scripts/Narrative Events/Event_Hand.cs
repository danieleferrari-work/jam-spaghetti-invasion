using System.Collections;
using UnityEngine;

public class Event_Hand : MonoBehaviour
{
    [SerializeField] float minLifetime;
    [SerializeField] float maxLifetime;

    float lifetime;

    void Awake()
    {
        lifetime = Random.Range(minLifetime, maxLifetime);

        StartCoroutine(Lifetime());
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
