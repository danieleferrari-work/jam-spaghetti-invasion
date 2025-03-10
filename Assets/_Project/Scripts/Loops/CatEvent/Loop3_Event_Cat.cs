using System.Collections;
using UnityEngine;

public class Loop3_Event_Cat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GondolaAutoPilotArea autoPilotArea;
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] CatAnimationController catAnimatorController;

    // References
    Loop3 loop;

    // Local Variables
    Coroutine jumpCatCoroutine;
    bool isLastJump;

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
        jumpCatCoroutine = StartCoroutine(PlayCatAnimation());
    }

    void OnEndAutoPilotMoving()
    {
        catAnimatorController.DoJumpOnBoat(FindObjectOfType<Gondola>().catOnBoat);
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

    IEnumerator PlayCatAnimation()
    {
        for (int i = 0; i < loop.catJumpRepetitions; i++)
        {
            yield return new WaitForSeconds(loop.catJumpPause);

            catAnimatorController.DoJump();
        }
    }
}
