using UnityEngine;

public class GondolaOscillator : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float bobFrequency = 1.0f;
    [SerializeField] private float bobAmplitude = 0.1f;

    [Header("Rolling Settings")]
    [SerializeField] private float rollFrequency = 0.5f;
    [SerializeField] private float rollAmplitude = 2.0f;

    [Header("Ground Settings")]
    [SerializeField] private float heightOffset = 2f;


    private float timeOffset;

    private void Start()
    {
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    public float CalculateOscillationRoll()
    {
        float time = Time.time + timeOffset;
        return Mathf.Sin(time * rollFrequency) * rollAmplitude;
    }

    public float CalculateOscillationBobbing()
    {
        float time = Time.time + timeOffset;
        float bobHeight = Mathf.Sin(time * bobFrequency) * bobAmplitude;
        return heightOffset + bobHeight;
    }
}