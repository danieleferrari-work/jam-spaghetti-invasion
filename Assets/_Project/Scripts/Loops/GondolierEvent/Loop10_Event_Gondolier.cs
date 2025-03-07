using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Loop10_Event_Gondolier : MonoBehaviour
{
    private CinemachineVirtualCamera cvcPlayer;
    [SerializeField] private GameObject mirroredGondola;
    private Transform modelPlayerGondola;
    private Transform followCamObj;

    private bool ActivatedEvent = false;
    Loop10 loop;
    private void Start()
    {
        cvcPlayer = FindFirstObjectByType<CinemachineVirtualCamera>();
       // mirroredGondola = GameObject.FindGameObjectWithTag("PlayerGondolaMirrored");
       // followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;

        loop = GetComponentInParent<Loop10>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerGondola"))
        {
            Instantiate(mirroredGondola, modelPlayerGondola);
            followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
            
            cvcPlayer.m_Follow = followCamObj;

            ActivatedEvent = true;

           // loop.gondolierEventCompleted = true;
        }
    }
}
