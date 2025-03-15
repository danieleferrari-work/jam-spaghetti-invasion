using Cinemachine;
using DevLocker.Utils;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Necessario per manipolare le immagini del Canvas

public class Loop10_Event_GondolierV2 : MonoBehaviour
{
    [SerializeField] private GameObject mirroredEnvironment;
    [SerializeField] private GameObject virtualCameraPrefab;
    [SerializeField] private GameObject mirroredEmptyGondola;
    [SerializeField] private float rotationTime;
    [SerializeField] private float delayBeforeNewCamera = 1f; // Tempo prima di instanziare la nuova Virtual Camera
    [SerializeField] private SceneReference negativeEndingScene;
    [SerializeField] private float TimeUntilFlip = 20f;
    private GameObject mainSceneEnv;
    private Transform modelPlayerGondola;
    private Transform playerParent;
    private FollowCameraRotation followCamObj;
    private Collider triggerCollider;
    private GondolaMovementManager movementManager;
    private float timePassed;
    private Vector3 lastPlayerPosition;  // Posizione precedente del player
    private bool hasMoved = false;  // Flag per sapere se il player si è mosso
    CinemachineVirtualCamera newVirtualCam;
    private bool positiveEndingUnlocked = false;
    // Canvas e Image da usare per il finale positivo
    [SerializeField] private Canvas positiveEndingCanvas;
    [SerializeField] private Image positiveEndingImage;

    Loop10 loop;

    private void Start()
    {
        modelPlayerGondola = GameObject.FindGameObjectWithTag("PlayerGondolaModel").transform;
        playerParent = modelPlayerGondola.parent;
        mainSceneEnv = GameObject.Find("Environment");
        triggerCollider = GetComponent<Collider>();
        movementManager = FindObjectOfType<GondolaMovementManager>();
        loop = GetComponentInParent<Loop10>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            movementManager.PauseMovement = true;
            if (triggerCollider != null)
                triggerCollider.enabled = false; // Disattiva subito il collider per evitare riattivazioni multiple
           // mirroredEmptyGondola.SetActive(false);
            mirroredEnvironment.SetActive(true);

            modelPlayerGondola.GetComponentInChildren<FollowCameraRotation>().enabled = false;

            CinemachineVirtualCamera highestCam = GetHighestPriorityCamera();

            if (highestCam != null)
            {
                StartCoroutine(RotatePlayerAndResetCamera(highestCam));
            }
            movementManager.IsFlipped = true;
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

        lastPlayerPosition = targetPosition;
    //    DisableEnvironmentChildrenExceptLight();

        highestCam.m_Lens.Dutch = highestCam.m_Lens.Dutch == 0 ? 180 : 0;

        if (followCamObj != null)
        {
            followCamObj.gameObject.SetActive(true);
        }

        StartCoroutine(DelayedCameraSpawn(highestCam.Priority + 1)); // Attende prima di creare la Virtual Camera
    }

    private IEnumerator DelayedCameraSpawn(int newPriority)
    {
        yield return new WaitForSeconds(delayBeforeNewCamera); // Attesa prima di instanziare la Virtual Camera

        if (virtualCameraPrefab != null)
        {
            newVirtualCam = Instantiate(virtualCameraPrefab, playerParent).GetComponent<CinemachineVirtualCamera>();
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

    public void CheckEnding()
    {
        if (!movementManager.IsFlipped) 
        {
            if (positiveEndingUnlocked)
            {
                ShowPositiveEnding(positiveEndingCanvas, positiveEndingImage);
            }
            else
            {
                //Finale con MORTE
                Debug.Log("La creatura uccide il player!");
            }

        }
        else
        {
            // Finale NEGATIVO
            loop.gondolierEventCompleted = true;
            //LoadNegativeEndingScene(negativeEndingScene.SceneName);
        }
    }

    private void ShowPositiveEnding(Canvas canvas, Image image)
    {
        canvas.gameObject.SetActive(true); // Attiva il Canvas

        // Coroutine per aumentare gradualmente l'alpha dell'immagine
        StartCoroutine(FadeInImage(image));
    }

    private IEnumerator FadeInImage(Image image)
    {
        float targetAlpha = 1f; // Alpha finale
        float currentAlpha = 0f; // Alpha iniziale
        float fadeSpeed = 0.5f; // Velocità di transizione

        Color startColor = image.color;
        startColor.a = currentAlpha;
        image.color = startColor;

        while (currentAlpha < targetAlpha)
        {
            currentAlpha += Time.deltaTime * fadeSpeed;
            startColor.a = Mathf.Clamp01(currentAlpha);
            image.color = startColor;
            yield return null;
        }
    }

    private void LoadNegativeEndingScene(string sceneName)
    {
        Debug.Log($"Caricamento scena speciale: {sceneName}");
        // var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //  load.completed += OnNegativeEndingSceneLoaded;
    }

    private void OnNegativeEndingSceneLoaded(AsyncOperation operation)
    {
        Debug.Log("Scena speciale caricata!");

        // Unload tutte le scene tranne quella appena caricata
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != negativeEndingScene.SceneName)
            {
                Debug.Log($"Scaricamento scena: {scene.name}");
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        LoopsManager.OnStartLoop?.Invoke();
    }

    private void Update()
    {
        if (movementManager.IsFlipped)
        {
            // Verifica se il player si è mosso
            if (Vector3.Distance(playerParent.position, lastPlayerPosition) > 1f)
            {
                hasMoved = true; // Se il player si muove, settiamo il flag
            }
            else
            {
                hasMoved = false;
            }

            // Se il player non si è mosso, incrementiamo il timer
            if (!hasMoved)
            {
                timePassed += Time.deltaTime;
            }

            // Se il timer raggiunge il limite, sblocchiamo il finale positivo
            if (timePassed >= TimeUntilFlip && !hasMoved)
            {
                //FLIP Gondola
                
                Debug.Log("Flip normal");
                float elapsedTime = 0f;
                Quaternion startRotation = playerParent.rotation;
                Quaternion targetRotation = Quaternion.Euler(0, 0, 0f);

                Vector3 startPosition = playerParent.position;
                Vector3 targetPosition = new Vector3(startPosition.x, 1f, startPosition.z);

                    float t = elapsedTime / rotationTime;
                    playerParent.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                    playerParent.position = Vector3.Lerp(startPosition, targetPosition, t);
                    elapsedTime += Time.deltaTime;

                playerParent.rotation = targetRotation;
                playerParent.position = targetPosition;
               
                if (followCamObj != null)
                {
                    followCamObj.gameObject.SetActive(true);
                }
                GameObject.FindGameObjectWithTag("PlayerEyes").transform.parent.localPosition = Vector3.zero;
                GameObject.FindGameObjectWithTag("PlayerEyes").transform.localPosition = new Vector3(0f, 1.6f, 0.2f);
                newVirtualCam.m_Priority = 0;
                movementManager.IsFlipped = false;
                positiveEndingUnlocked = true;
            }
        }
    }
}
