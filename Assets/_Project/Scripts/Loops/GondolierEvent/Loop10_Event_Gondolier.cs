using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Loop10_Event_Gondolier : MonoBehaviour
{
    private CinemachineVirtualCamera cvcPlayer;
    [SerializeField] private GameObject mirroredGondola;
    private GameObject mirroredGondolaLocal;
    private Transform modelPlayerGondola;
    private Transform followCamObj;
    private GondolaMovementManager movementManager;
    private bool ActivatedEvent = false;
    Loop10 loop;
    private void Start()
    {
        movementManager = FindFirstObjectByType<GondolaMovementManager>();
       //mirroredGondola = GameObject.FindGameObjectWithTag("PlayerGondolaMirrored");
       // followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;

        loop = GetComponentInParent<Loop10>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            mirroredGondolaLocal = Instantiate(mirroredGondola, modelPlayerGondola);
            followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
            cvcPlayer = mirroredGondolaLocal.GetComponentInChildren<CinemachineVirtualCamera>();
            cvcPlayer.m_Follow = followCamObj;
            cvcPlayer.m_Priority = 15;
            cvcPlayer.GetComponent<CinemachineInputProvider>().enabled = false; // Disabilita input

            ActivatedEvent = true;
            movementManager.IsFlipped = true;
            GetComponentInChildren<MeshRenderer>().enabled = false;

            // Avvia la coroutine per aspettare la fine della transizione
            //  StartCoroutine(WaitForCameraTransition());
            StartCoroutine(ReactivateInputAfterDelayOrTransition(4f));
        }
    }

    private IEnumerator ReactivateInputAfterDelayOrTransition(float delay)
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        float timer = 0f;
        while ((brain.ActiveBlend != null || timer < delay))
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (cvcPlayer != null)
        {
            cvcPlayer.GetComponent<CinemachineInputProvider>().enabled = true;
        }
    }

}
