using System.Collections;
using BaseTemplate;
using UnityEngine;

public class Params : Singleton<Params>
{
    protected override bool isDontDestroyOnLoad => false;

    [Header("Player Zoom Settings")]
    public float minFov;
    public float zoomInSpeed;
    public float zoomOutSpeed;
}
