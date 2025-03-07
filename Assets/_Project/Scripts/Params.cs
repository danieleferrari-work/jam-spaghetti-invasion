using BaseTemplate;
using UnityEngine;

public class Params : Singleton<Params>
{
    protected override bool isDontDestroyOnLoad => false;

    [Header("Player Zoom Settings")]
    public float minFov;
    public float zoomInSpeed;
    public float zoomOutSpeed;

    [Header("Gondola Movement Settings")]

    public float gondolaMaxSpeed;
    public float gondolaRotationSpeed;
    public float gondolaDefaultAcceleration;
    public float rowPushForce;
    public float rowPushDelay;
    public float rowPushDuration;

    [Header("Bobbing Settings")]
    public float gondolaBobbingFrequency;
    public float gondolaBobbingAltitude;

    [Header("Rolling Settings")]
    public float gondolaRollingFrequency;
    public float gondolaRollingAmplitude;
}
