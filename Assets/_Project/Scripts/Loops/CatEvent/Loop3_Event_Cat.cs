using System.Collections;
using UnityEngine;

public class Loop3_Event_Cat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GondolaAutoPilotArea autoPilotArea;
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] CatAnimationController catAnimatorController;
    [SerializeField] GameObject cat;

    // References
    Loop3 loop;

    // Local Variables
    Coroutine jumpCatCoroutine;

    void Awake()
    {
        loop = GetComponentInParent<Loop3>();

        if (loop.catEventCompleted)
        {
            Destroy(gameObject);
        }

        watchEvent.OnEventStarted += StartJumping;
        autoPilotArea.OnStartMoving += OnStartAutoPilotMoving;
        autoPilotArea.OnEndMoving += OnEndAutoPilotMoving;
        catAnimatorController.OnJumpOnBoatFinished += CatJumpOnBoatFinished;
    }

    void StartJumping()
    {
        jumpCatCoroutine = StartCoroutine(PlayCatJumpAnimation());
    }

    void OnEndAutoPilotMoving()
    {
        cat.transform.SetParent(FindObjectOfType<Gondola>().catOnBoat.transform, worldPositionStays: true);
        catAnimatorController.DoJumpOnBoat();
    }

    void OnStartAutoPilotMoving()
    {
        if (jumpCatCoroutine != null)
            StopCoroutine(jumpCatCoroutine);
        FindObjectOfType<GondolaFloatingManager>().StopFloating();
    }

    public void CatJumpOnBoatFinished()
    {
        loop.catEventCompleted = true;
        FindObjectOfType<GondolaFloatingManager>().StartFloating();
    }

    IEnumerator PlayCatJumpAnimation()
    {
        for (int i = 0; i < loop.catJumpRepetitions; i++)
        {
            yield return new WaitForSeconds(loop.catJumpPause);

            catAnimatorController.DoJump();
        }
    }
}
