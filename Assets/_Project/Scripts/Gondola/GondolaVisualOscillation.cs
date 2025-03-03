using UnityEngine;

public class GondolaVisualOscillation : MonoBehaviour
{
    [SerializeField] private GondolaFloatingManager floatingManager;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        // Salva la posizione e la rotazione iniziali dell'oggetto figlio
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        // Calcola i valori di oscillazione dal floating manager
        float bobbing = floatingManager.CalculateOscillationBobbing();
        float roll = floatingManager.CalculateOscillationRoll();
      
        transform.localPosition = new Vector3(initialLocalPosition.x, bobbing, initialLocalPosition.z);

        // Applica l'oscillazione di roll (rotazione sull'asse Z)
        transform.localRotation = initialLocalRotation * Quaternion.Euler(0, 0, roll);
    }
}
