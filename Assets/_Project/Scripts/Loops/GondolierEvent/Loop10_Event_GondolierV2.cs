using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Loop10_Event_GondolierV2 : MonoBehaviour
{
    private CinemachineVirtualCamera cvcPlayer;
    private GameObject mirroredGondola;
    private GameObject mirroredGondolaLocal;
    private Transform modelPlayerGondola;
    private Transform followCamObj;

    private bool ActivatedEvent = false;
    Loop10 loop;
    private void Start()
    {
        
       //mirroredGondola = GameObject.FindGameObjectWithTag("PlayerGondolaMirrored");
       // followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;

        loop = GetComponentInParent<Loop10>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerGondola"))
        {
            mirroredGondolaLocal = Instantiate(mirroredGondola, modelPlayerGondola);
            followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
            cvcPlayer = mirroredGondolaLocal.GetComponentInChildren<CinemachineVirtualCamera>();
            cvcPlayer.m_Follow = followCamObj;
            cvcPlayer.m_Priority = 15;
            ActivatedEvent = true;

           // loop.gondolierEventCompleted = true;
        }
    }

}
