using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGondolaDisapearing : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy = 3f;
    [SerializeField] private Vector3 acceleration = new Vector3(0f, 1f, 0f); // Accelerazione costante
    private Vector3 velocity = Vector3.zero; // Velocit� iniziale

    void Start()
    {
        // Distrugge l'oggetto dopo il tempo specificato
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        // Aggiorna la velocit� applicando l'accelerazione
        velocity += acceleration * Time.deltaTime;
        // Aggiorna la posizione in base alla velocit� corrente
        transform.position += velocity * Time.deltaTime;
    }
}
