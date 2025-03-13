using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; // Necessario per il componente Volume

public class Loop10_Event_Gondolier : MonoBehaviour
{
    private CinemachineVirtualCamera cvcPlayer;
    [SerializeField] private GameObject mirroredPlayerGondola;
    [SerializeField] private GameObject mirroredEmptyGondola;
    [SerializeField] private GameObject mirroredEnvironment;
    private GameObject mainSceneEnv;
    private GameObject mirroredPlayerGondolaLocal;
    private Transform modelPlayerGondola;
    private Transform followCamObj;
    private GondolaMovementManager movementManager;
    private bool ActivatedEvent = false;
    Loop10 loop;

    private void Start()
    {
        movementManager = FindFirstObjectByType<GondolaMovementManager>();
        // Trova il modello della gondola associata al player tramite tag
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;
        // Trova l'oggetto Environment nella scena
        mainSceneEnv = GameObject.Find("Environment");
        loop = GetComponentInParent<Loop10>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            // Cerca la virtual camera con la priorità più alta nella scena
            CinemachineVirtualCamera[] allCams = FindObjectsOfType<CinemachineVirtualCamera>();
            CinemachineVirtualCamera highestCam = null;
            int highestPriority = int.MinValue;
            foreach (var cam in allCams)
            {
                if (cam.Priority > highestPriority)
                {
                    highestPriority = cam.Priority;
                    highestCam = cam;
                }
            }

            if (highestCam != null)
            {
                // Disabilita l'inputProvider della camera trovata
                CinemachineInputProvider inputProvider = highestCam.GetComponent<CinemachineInputProvider>();
                if (inputProvider != null)
                {
                    inputProvider.enabled = false;
                }
                // Imposta la rotazione/direzione desiderata usando la forward dell'oggetto che contiene questo script
                highestCam.transform.rotation = Quaternion.LookRotation(modelPlayerGondola.forward);
            }

            Light directionalLight = mainSceneEnv.GetComponentInChildren<Light>(true);
            if (directionalLight != null && directionalLight.type == UnityEngine.LightType.Directional)
            {
                HDAdditionalLightData hdLightData = directionalLight.GetComponent<HDAdditionalLightData>();
                if (hdLightData != null)
                {
                    hdLightData.angularDiameter = 0f;
                }
            }

            // Attiva l'environment speculare
            mirroredEnvironment.SetActive(true);
            // Istanzia la gondola mirroring in corrispondenza del modello
            mirroredPlayerGondolaLocal = Instantiate(mirroredPlayerGondola, modelPlayerGondola);
            // Trova il transform del follow della camera tramite tag
            followCamObj = GameObject.FindGameObjectWithTag("PlayerGondolaMirroredEyes").transform;
            cvcPlayer = mirroredPlayerGondolaLocal.GetComponentInChildren<CinemachineVirtualCamera>();
            cvcPlayer.m_Follow = followCamObj;
            // Imposta la priorità della nuova camera: quella trovata + 1
            cvcPlayer.m_Priority = highestPriority + 1;
            // Disabilita l'input temporaneamente
            cvcPlayer.GetComponent<CinemachineInputProvider>().enabled = false;

            ActivatedEvent = true;
            movementManager.IsFlipped = true;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            // Avvia la coroutine che riattiva l'input dopo la transizione o un ritardo
            StartCoroutine(ReactivateInputAfterDelayOrTransition(3f));
        }
    }

    private IEnumerator ReactivateInputAfterDelayOrTransition(float delay)
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        float timer = 0f;
        while (brain.ActiveBlend != null || timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (cvcPlayer != null)
        {
            // Riattiva l'input
            cvcPlayer.GetComponent<CinemachineInputProvider>().enabled = true;
            
            // Disattiva tutti i figli di Environment tranne quelli con Collider,
            // quelli con Directional Light e il Global Volume (e i relativi antenati)
            DeactivateChildrenExceptSpecial();
        }
    }

    private void DeactivateChildrenExceptSpecial()
    {
        if (mainSceneEnv == null) return;

        // HashSet per memorizzare i Transform che devono rimanere attivi
        HashSet<Transform> activeTransforms = new HashSet<Transform>();

        // Ottieni tutti i Transform discendenti (anche quelli inattivi)
        Transform[] allDescendants = mainSceneEnv.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allDescendants)
        {
            if (t == mainSceneEnv.transform)
                continue;

            bool hasCollider = t.GetComponent<Collider>() != null;

            Light lightComponent = t.GetComponent<Light>();
            bool isDirectionalLight = (lightComponent != null && lightComponent.type == UnityEngine.LightType.Directional);

            Volume volumeComponent = t.GetComponent<Volume>();
            bool isGlobalVolume = (volumeComponent != null && volumeComponent.isGlobal);

            // Se il GameObject ha un Collider oppure possiede una Light di tipo Directional
            // oppure possiede un Global Volume, aggiungi anche i suoi antenati (fino a "Environment")
            if (hasCollider || isDirectionalLight || isGlobalVolume)
            {
                Transform current = t;
                while (current != null && current != mainSceneEnv.transform)
                {
                    activeTransforms.Add(current);
                    current = current.parent;
                }
            }
        }

        // Applica ricorsivamente lo stato attivo/inattivo basato sul set calcolato
        SetActiveRecursively(mainSceneEnv.transform, activeTransforms);
    }

    private void SetActiveRecursively(Transform parent, HashSet<Transform> activeSet)
    {
        foreach (Transform child in parent)
        {
            // Se il figlio è nel set, deve rimanere attivo
            bool shouldBeActive = activeSet.Contains(child);
            child.gameObject.SetActive(shouldBeActive);
            SetActiveRecursively(child, activeSet);
        }
    }
}
