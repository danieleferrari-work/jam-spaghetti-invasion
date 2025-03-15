using UnityEngine;
using System.Collections;
using Cinemachine;

public class ChangePovEffect : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Animator anim;
    [SerializeField] private float cameraSwitchTransitionDuration = 1f;



    private CinemachineVirtualCamera playerCamera;

    void Awake()
    {
        playerCamera = PlayerCameraManager.instance.PlayerPovVirtualCamera;
    }

    /// <summary>
    /// Immediately switch to the destination camera
    /// </summary>
    public void ChangeCamera(CinemachineVirtualCamera destinationCamera)
    {
        StartCoroutine(SwitchCameraCoroutine(destinationCamera));
    }

    /// <summary>
    /// Reset back to the player's camera
    /// </summary>
    public void ResetCamera()
    {
        StartCoroutine(SwitchCameraCoroutine(playerCamera));
    }

    public void StartShaking()
    {
        PlayerCameraManager.instance.StartShaking();
    }

    public void StopShaking()
    {
        PlayerCameraManager.instance.StopShaking();
    }

    private IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera targetCamera)
    {
        if (anim != null)
            anim.SetTrigger("ChangeCam");

        yield return new WaitForSeconds(cameraSwitchTransitionDuration);
        PlayerCameraManager.instance.ChangeCamera(targetCamera);
    }

}