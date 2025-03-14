using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; // Necessario per gestire HDAdditionalLightData

public class Loop10_Event_GondolierV2 : MonoBehaviour
{
    [SerializeField] private GameObject mirroredEnvironment;
    [SerializeField] private GameObject virtualCameraPrefab;
    [SerializeField] private GameObject mirroredEmptyGondola;
    [SerializeField] private float rotationTime;
    [SerializeField] private float delayBeforeNewCamera = 1f; // Tempo prima di instanziare la nuova Virtual Camera
    private GameObject mainSceneEnv;
    private Transform modelPlayerGondola;
    private Transform playerParent;
    private FollowCameraRotation followCamObj;
    private Collider triggerCollider;
    private GondolaMovementManager movementManager;
    private Volume fogVolume;
    private Fog fogComponent;
    private void Start()
    {
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;
        playerParent = modelPlayerGondola.parent;
        mainSceneEnv = GameObject.Find("Environment");
        triggerCollider = GetComponent<Collider>();
        movementManager = FindObjectOfType<GondolaMovementManager>();   
        fogVolume = GetComponentInChildren<Volume>();
        if (fogVolume != null)
        {
            fogVolume.profile.TryGet(out fogComponent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            movementManager.PauseMovement = true;
            if (triggerCollider != null)
                triggerCollider.enabled = false; // Disattiva subito il collider per evitare riattivazioni multiple
            mirroredEmptyGondola.SetActive(false);
            mirroredEnvironment.SetActive(true);

            modelPlayerGondola.GetComponentInChildren<FollowCameraRotation>().enabled = false;

            CinemachineVirtualCamera highestCam = GetHighestPriorityCamera();

            if (highestCam != null)
            {
                StartCoroutine(RotatePlayerAndResetCamera(highestCam));
                
            }
        }
    }

    private void DisableEnvironmentChildrenExceptLight()
    {
        foreach (Transform child in mainSceneEnv.transform)
        {
            Light light = child.gameObject.GetComponent<Light>();
            if (light != null && light.type == LightType.Directional)
            {
                HDAdditionalLightData hdLightData = light.GetComponent<HDAdditionalLightData>();
                if (hdLightData != null)
                {
                    hdLightData.angularDiameter = 0f; // Disattiva Angular Diameter se HDRP
                }
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private CinemachineVirtualCamera GetHighestPriorityCamera()
    {
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
        return highestCam;
    }

    private IEnumerator RotatePlayerAndResetCamera(CinemachineVirtualCamera highestCam)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = playerParent.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, 180f);

        Vector3 startPosition = playerParent.position;
        Vector3 targetPosition = new Vector3(startPosition.x, -4f, startPosition.z);

        while (elapsedTime < rotationTime)
        {
            float t = elapsedTime / rotationTime;
            playerParent.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            playerParent.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerParent.rotation = targetRotation;
        playerParent.position = targetPosition;
        DisableEnvironmentChildrenExceptLight();

        highestCam.m_Lens.Dutch = highestCam.m_Lens.Dutch == 0 ? 180 : 0;

        if (followCamObj != null)
        {
            followCamObj.gameObject.SetActive(true);
        }
        // Effetto graduale sulla nebbia prima della Virtual Camera
        if (fogComponent != null)
        {
            yield return StartCoroutine(FadeFogDistance(fogComponent, 50f, 2f)); // 2 secondi di fade
        }

        StartCoroutine(DelayedCameraSpawn(highestCam.Priority + 1)); // Attende prima di creare la Virtual Camera
    }
    private IEnumerator FadeFogDistance(Fog fog, float targetValue, float duration)
{
    float elapsedTime = 0f;
    float startValue = fog.meanFreePath.value; // Valore attuale della nebbia

    while (elapsedTime < duration)
    {
        float t = elapsedTime / duration;
        fog.meanFreePath.value = Mathf.Lerp(startValue, targetValue, t);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    fog.meanFreePath.value = targetValue; // Assicura che arrivi al valore finale
}


    private IEnumerator DelayedCameraSpawn(int newPriority)
    {
        yield return new WaitForSeconds(delayBeforeNewCamera); // Attesa prima di instanziare la Virtual Camera

        if (virtualCameraPrefab != null)
        {
            CinemachineVirtualCamera newVirtualCam = Instantiate(virtualCameraPrefab, playerParent).GetComponent<CinemachineVirtualCamera>();
            GameObject.FindGameObjectWithTag("PlayerEyes").transform.parent.localPosition = Vector3.zero;
            GameObject.FindGameObjectWithTag("PlayerEyes").transform.localPosition = new Vector3(-0.2f, -1.8f, 0);
            newVirtualCam.m_Follow = GameObject.FindGameObjectWithTag("PlayerEyes").transform;
            newVirtualCam.m_LookAt = modelPlayerGondola.transform;
            newVirtualCam.Priority = newPriority; // Assegna la nuova priorit� 

        }
        movementManager.IsFlipped = true;
        movementManager.PauseMovement = false;
        
        modelPlayerGondola.GetComponentInChildren<FollowCameraRotation>().enabled = true;

    }

}
