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
    public float bobbingSpeed;
    public float gondolaBobbingFrequency;
    public float gondolaBobbingAltitude;

    [Header("Gondola Rolling Settings")]
    public float rollingSpeed;
    public float gondolaRollingFrequency;
    public float gondolaRollingAmplitude;

    [Header("Event Hands Settings")]

    [Tooltip("Hand spawn radius around boat")]
    public float handsEventRadius;

    [Tooltip("Hand spawn min height (0 is on the water level)")]
    public float handsEventMinHeight;

    [Tooltip("Hand spawn max height (0 is on the water level)")]
    public float handsEventMaxHeight;

    [Tooltip("Number of hands to spawn every spawnDelay")]
    public int handsEventSpawnCount;

    [Tooltip("Number of seconds between hands spawns")]
    public int handsEventSpawnDelay;
}
