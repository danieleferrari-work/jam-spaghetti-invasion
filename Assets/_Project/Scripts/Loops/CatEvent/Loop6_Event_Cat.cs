using System;
using UnityEngine;

public class Loop6_Event_Cat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDuration;

    [Header("References")]
    [SerializeField] GameObject catOnGround;
    [SerializeField] GameObject catShadowOnGround;
    [SerializeField] GondolaAutoPilotArea autoPilotArea;

    // References
    Loop6 loop;
    GameObject catOnBoat;
    CatAnimationController animationController;


    void Awake()
    {
        loop = GetComponentInParent<Loop6>();
        catOnBoat = FindObjectOfType<Gondola>().catOnBoat;
        autoPilotArea.OnStartMoving += OnStartAutoPilotMoving;
        autoPilotArea.OnEndMoving += OnEndAutoPilotMoving;
        animationController = catOnBoat.GetComponentInChildren<CatAnimationController>();
    }

    private void OnEndAutoPilotMoving()
    {
        animationController.gameObject.transform.SetParent(catOnBoat.transform, worldPositionStays: true);
        animationController.DoExit();
        animationController.OnExitFinished += OnExitFinished;
    }

    private void OnStartAutoPilotMoving()
    {
        FindObjectOfType<GondolaFloatingManager>().StopFloating(); // TO forse mettere direttamtne nell'autopilot
    }

    private void OnExitFinished()
    {
        catShadowOnGround.gameObject.SetActive(false);
        loop.catEventCompleted = true;
    }
}
