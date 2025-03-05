using UnityEngine;

public class GondolaFloatingManager : MonoBehaviour
{
    [SerializeField] GameObject model;

    [Header("Bobbing Settings")]
    [SerializeField] float bobFrequency = 1.0f;
    [SerializeField] float bobAmplitude = 0.1f;

    [Header("Rolling Settings")]
    [SerializeField] float rollFrequency = 0.5f;
    [SerializeField] float rollAmplitude = 2.0f;

    [Header("Ground Settings")]
    [SerializeField] float heightOffset = 2f;


    float timeOffset;

    private void Start()
    {
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void FixedUpdate()
    {
        ApplyOscillationBobbing();
        ApplyOscillationRoll();
    }

    private void ApplyOscillationRoll()
    {
        float time = Time.time + timeOffset;
        var roll = Mathf.Sin(time * rollFrequency) * rollAmplitude; ;
        model.transform.rotation = Quaternion.Euler(new Vector3(model.transform.rotation.eulerAngles.x, model.transform.rotation.eulerAngles.y, roll));
    }

    private void ApplyOscillationBobbing()
    {
        float time = Time.time + timeOffset;
        float bobHeight = Mathf.Sin(time * bobFrequency) * bobAmplitude;
        var bobbing = heightOffset + bobHeight; ;
        model.transform.position = new Vector3(model.transform.position.x, bobbing, model.transform.position.z);
    }

}