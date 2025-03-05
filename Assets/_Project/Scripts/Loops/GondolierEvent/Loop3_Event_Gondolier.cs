using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Loop3_Event_Gondolier : MonoBehaviour
{
    public float rayDistance = 100f; // Distanza massima del raycast
    private CinemachineVirtualCamera cmVCam;

    void Start()
    {
       
    }

    void Update()
    {
        IsBeingLookedAt();
    }

    void IsBeingLookedAt()
    {
        if (cmVCam == null) return;

        Vector3 dirToObject = (transform.position - cmVCam.transform.position).normalized;
        float dot = Vector3.Dot(cmVCam.transform.forward, dirToObject);

        if (dot > 0.9f) // 1.0 = perfettamente davanti, 0.9 = dentro il campo visivo
        {
            Debug.Log(gameObject.name + " è nel campo visivo del player!");
        }
    }

}
