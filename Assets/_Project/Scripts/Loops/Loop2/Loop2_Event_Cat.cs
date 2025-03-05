using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Loop2_Event_Cat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDuration;

    [Header("References")]
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] GameObject cat;
    [SerializeField] GameObject catJumpEndPosition;

    // References
    Loop2 loop;

    // Local Variables
    Vector3 startPosition;
    Coroutine jumpCatCoroutine;
    Sequence jumpCatSequence;


    void Awake()
    {
        loop = GetComponentInParent<Loop2>();
        startPosition = cat.transform.position;

        watchEvent.OnEventSuccessed += StartJumping;
    }

    void StartJumping()
    {
        Loop2_Cat.OnCatJumpedOnBoat += OnCatJumpedOnBoat;
        jumpCatCoroutine = StartCoroutine(PlayCatAnimation());
    }

    void OnCatJumpedOnBoat()
    {
        StopCoroutine(jumpCatCoroutine);

        loop.catEventCompleted = true;

        jumpCatSequence.Kill();
        Destroy(gameObject);
    }


    IEnumerator PlayCatAnimation()
    {
        for (int i = 0; i < loop.catJumpRepetitions; i++)
        {
            cat.transform.position = startPosition;

            yield return new WaitForSeconds(loop.catJumpPause);

            jumpCatSequence = cat.transform.DOJump(catJumpEndPosition.transform.position, jumpForce, 1, jumpDuration);

            yield return new WaitForSeconds(jumpDuration);
        }

        Destroy(gameObject);
    }
}
