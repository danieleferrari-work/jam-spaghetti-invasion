using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangePovEffect : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam,mainCam;
    [SerializeField] private Camera camC, mainCamC;
    [SerializeField] private Animator anim;
    [SerializeField] private float fovForAnim,maxFov,MaxAnimationFov,animSpeed,fovSpeed,animationTime;
    private float fov, fov2;
    [SerializeField] private bool hasSeenPlayer = false;
    private bool hasAnimStarted = true;
    private void Start()
    {
        camC.gameObject.SetActive(false);
        mainCamC.gameObject.SetActive(true);
        fov = cam.m_Lens.FieldOfView;
        fov2 = maxFov;
    }
    private void Update()
    {
        if (hasSeenPlayer)
        {
            if (fov > maxFov)
                StarFovEffect();
            else
            {
                if (fov2 > MaxAnimationFov)
                    FinishFovEffect();
                else
                    StartCoroutine(ChangeCam());
            }
        }
        else 
        {
            RestetFov();
        }
    }
    private void StarFovEffect()
    {
        fov -= Time.deltaTime * animSpeed;
        cam.m_Lens.FieldOfView = fov;
    }
    private void FinishFovEffect()
    {
        fov2 -= Time.deltaTime * fovSpeed;
        cam.m_Lens.FieldOfView = fov2;
    }
    private void RestetFov()
    {

        fov += Time.deltaTime * animSpeed*10;
        cam.m_Lens.FieldOfView = fov;
        if (fov >= 70) 
        {
            cam.m_Lens.FieldOfView = 70;
            fov = 70;
        }
    }
    private IEnumerator ChangeCam()
    {
        if (hasAnimStarted)
        {
            anim.SetTrigger("ChangeCam");
            yield return new WaitForSeconds(animationTime);
            camC.gameObject.SetActive(true);
            mainCamC.gameObject.SetActive(false);
            hasAnimStarted = false;
        }else
            yield return null;

    }
}
