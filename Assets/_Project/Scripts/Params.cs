using BaseTemplate;
using UnityEngine;

public class Params : Singleton<Params>
{
    protected override bool isDontDestroyOnLoad => false;

    [Header("Player Zoom Settings")]
    public float minFov;
    public float zoomInSpeed;
    public float zoomOutSpeed;
    
    [Space(10)]
    [Header("Gondola Movement Settings")]
    public float gondolaMaxSpeed;
    public float gondolaRotationSpeed;
    public float gondolaDefaultAcceleration;

    [Header("Gondola Rowing Settings")]
    public float rowPushForce;
    public float rowPushDelay;
    public float rowPushDuration;

    [Header("Gondola Bobbing Settings")]
    public float gondolaBobbingFrequency;
    public float gondolaBobbingAltitude;

    [Header("Gondola Rolling Settings")]
    public float gondolaRollingFrequency;
    public float gondolaRollingAmplitude;
}
