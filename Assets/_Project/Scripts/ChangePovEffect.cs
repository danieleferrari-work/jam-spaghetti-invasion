using System.Collections;
using UnityEngine;
using Cinemachine;

public class ChangePovEffect : MonoBehaviour
{
    public bool hasSeenPlayer;
    [SerializeField] private CinemachineVirtualCamera destinationCamera;
    [SerializeField] private Animator anim;
    [SerializeField] private float maxFov;
    [SerializeField] private float maxAnimationFov;
    [SerializeField] private float animSpeed;
    [SerializeField] private float fovSpeed;
    [SerializeField] private float cameraSwitchTransitionDuration;

    private float fov, fov2;
    private CinemachineVirtualCamera playerCamera;


    void Awake()
    {
        playerCamera = PlayerCameraManager.instance.PlayerPovVirtualCamera;
    }

    public void ResetCamera()
    {
        StartCoroutine(SwitchCameraCoroutine(playerCamera));
    }

    public void ChangeCamera()
    {
        StartCoroutine(SwitchCameraCoroutine(destinationCamera));
    }

    private void Start()
    {
        destinationCamera.gameObject.SetActive(false);
        fov = playerCamera.m_Lens.FieldOfView;
        fov2 = maxFov;
    }

    private void Update()
    {
        if (hasSeenPlayer)
            ApplyEffect();
        else
            RevertEffect();
    }

    private void ApplyEffect()
    {
        if (fov > maxFov)
        {
            fov -= Time.deltaTime * animSpeed;
            playerCamera.m_Lens.FieldOfView = fov;
        }
        else if (fov2 > maxAnimationFov)
        {
            fov2 -= Time.deltaTime * fovSpeed;
            playerCamera.m_Lens.FieldOfView = fov2;
        }
    }

    private void RevertEffect()
    {
        fov += Time.deltaTime * animSpeed * 10;
        playerCamera.m_Lens.FieldOfView = fov;
        if (fov >= 70)
        {
            playerCamera.m_Lens.FieldOfView = 70;
            fov = 70;
        }
    }

    private IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera destinationCamera)
    {
        anim.SetTrigger("ChangeCam");
        yield return new WaitForSeconds(cameraSwitchTransitionDuration);

        PlayerCameraManager.instance.ChangeCamera(destinationCamera);

        Destroy(gameObject);
    }
}
