using UnityEngine;

public class GondolaFloatingManager : MonoBehaviour
{
    [SerializeField] GameObject model;

    float timeOffset;
    bool floatingEnabled = true;

    public void StopFloating()
    {
        floatingEnabled = false;
    }

    public void StartFloating()
    {
        floatingEnabled = true;
    }

    private void Start()
    {
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void FixedUpdate()
    {
        if (floatingEnabled)
        {
            ApplyBobbing();
            ApplyRoll();
        }
        else
        {
            StopRolling();
            StopBobbing();
        }
    }

    void StopRolling()
    {
        model.transform.localRotation = Quaternion.Lerp(model.transform.localRotation, Quaternion.identity, Time.deltaTime);
    }

    void StopBobbing()
    {
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, Vector3.zero, Time.deltaTime);
    }

    void ApplyRoll()
    {
        float time = Time.time + timeOffset;
        var roll = Mathf.Sin(time * Params.instance.gondolaRollingFrequency) * Params.instance.gondolaRollingAmplitude;
        var targetRotation = Quaternion.Euler(new Vector3(model.transform.localRotation.eulerAngles.x, model.transform.localRotation.eulerAngles.y, roll));

        model.transform.localRotation = Quaternion.Lerp(model.transform.localRotation, targetRotation, Time.deltaTime * Params.instance.rollingSpeed);
    }

    void ApplyBobbing()
    {
        float time = Time.time + timeOffset;
        float bobHeight = Mathf.Sin(time * Params.instance.gondolaBobbingFrequency) * Params.instance.gondolaBobbingAltitude;
        var targetTransform = new Vector3(model.transform.localPosition.x, bobHeight, model.transform.localPosition.z);

        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, targetTransform, Time.deltaTime * Params.instance.bobbingSpeed);
    }
}