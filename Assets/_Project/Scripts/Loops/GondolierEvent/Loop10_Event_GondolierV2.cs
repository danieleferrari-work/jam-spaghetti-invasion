using System.Collections;
using Cinemachine;
using UnityEngine;
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

    private void Start()
    {
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;
        playerParent = modelPlayerGondola.parent;
        mainSceneEnv = GameObject.Find("Environment");
        triggerCollider = GetComponent<Collider>();
        movementManager = FindObjectOfType<GondolaMovementManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            if (triggerCollider != null)
                triggerCollider.enabled = false; // Disattiva subito il collider per evitare riattivazioni multiple
            mirroredEmptyGondola.SetActive(false);
            mirroredEnvironment.SetActive(true);
            FindObjectOfType<FollowCameraRotation>().enabled = false;

            CinemachineVirtualCamera highestCam = GetHighestPriorityCamera();

            if (highestCam != null)
            {
                followCamObj = FindObjectOfType<FollowCameraRotation>();
                if (followCamObj != null)
                {
                    followCamObj.gameObject.SetActive(false);
                }
                StartCoroutine(RotatePlayerAndResetCamera(highestCam));
                StartCoroutine(DelayedCameraSpawn(highestCam.Priority + 1)); // Attende prima di creare la Virtual Camera
                movementManager.IsFlipped = true;
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
        Vector3 targetPosition = new Vector3(startPosition.x, -3f, startPosition.z);

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
    }

    private IEnumerator DelayedCameraSpawn(int newPriority)
    {
        yield return new WaitForSeconds(delayBeforeNewCamera); // Attesa prima di instanziare la Virtual Camera

        if (virtualCameraPrefab != null)
        {
            GameObject newVirtualCam = Instantiate(virtualCameraPrefab, playerParent);
            newVirtualCam.GetComponent<CinemachineVirtualCamera>().Priority = newPriority; // Assegna la nuova priorit�
        }
    }
}
