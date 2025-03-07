using UnityEngine;

public class GondolaFloatingManager : MonoBehaviour
{
    [SerializeField] GameObject model;

    [Tooltip("Distance from the water")]
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
        var roll = Mathf.Sin(time * Params.instance.gondolaRollingFrequency) * Params.instance.gondolaRollingAmplitude; ;
        model.transform.rotation = Quaternion.Euler(new Vector3(model.transform.rotation.eulerAngles.x, model.transform.rotation.eulerAngles.y, roll));
    }

    private void ApplyOscillationBobbing()
    {
        float time = Time.time + timeOffset;
        float bobHeight = Mathf.Sin(time * Params.instance.gondolaBobbingFrequency) * Params.instance.gondolaBobbingAltitude;
        var bobbing = heightOffset + bobHeight; ;
        model.transform.position = new Vector3(model.transform.position.x, bobbing, model.transform.position.z);
    }

}