using System.Collections;
using UnityEngine;

public class Loop3_Event_Cat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GondolaAutoPilotArea autoPilotArea;
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] Animator catAnimator;
    [SerializeField] CatJumpStateMachine catJumpStateMachine;

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
    }

    void StartJumping()
    {
        jumpCatCoroutine = StartCoroutine(PlayCatAnimation());
    }

    void OnEndAutoPilotMoving()
    {
        catAnimator.SetTrigger("DoJump");
        isLastJump = true;
    }

    void OnStartAutoPilotMoving()
    {
        if (jumpCatCoroutine != null)
            StopCoroutine(jumpCatCoroutine);
    }

    public void CatJumpFinished()
    {
        if (isLastJump)
        {
            loop.catEventCompleted = true;
            Destroy(gameObject);
        }
    }

    IEnumerator PlayCatAnimation()
    {
        for (int i = 0; i < loop.catJumpRepetitions; i++)
        {
            yield return new WaitForSeconds(loop.catJumpPause);

            catAnimator.SetTrigger("DoJump");
        }
    }
}
