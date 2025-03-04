using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Event_Cat_Loop2 : MonoBehaviour
{
    [SerializeField] GameObject cat;
    [SerializeField] GameObject catJumpEndPosition;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDuration;
    [SerializeField] float jumpDelay = 5;
    [SerializeField] float jumpPause = 2;
    [SerializeField] float jumpsReplay = 3;

    Vector3 startPosition;
    Coroutine jumpCatCoroutine;
    Sequence jumpCatSequence;

    void Awake()
    {
        startPosition = cat.transform.position;

        Cat.OnCatJumpedOnBoat += OnCatJumpedOnBoat;
    }

    private void OnCatJumpedOnBoat()
    {
        StopCoroutine(jumpCatCoroutine);
        jumpCatSequence.Kill();
        Destroy(cat);
        FindFirstObjectByType<Gondola>().catOnBoat.SetActive(true);
    }

    void Start()
    {
        jumpCatCoroutine = StartCoroutine(PlayCatAnimation());
    }

    private IEnumerator PlayCatAnimation()
    {
        for (int i = 0; i < jumpsReplay; i++)
        {
            cat.transform.position = startPosition;

            yield return new WaitForSeconds(jumpPause);

            jumpCatSequence = cat.transform.DOJump(catJumpEndPosition.transform.position, jumpForce, 1, jumpDuration);

            yield return new WaitForSeconds(jumpDuration);
        }

        Destroy(gameObject);
    }
}
