using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Loop2_Event_Cat : MonoBehaviour
{
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] GameObject cat;
    [SerializeField] GameObject catJumpEndPosition;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDuration;

    Vector3 startPosition;
    Coroutine jumpCatCoroutine;
    Sequence jumpCatSequence;
    Loop2 loop;

    void Awake()
    {
        loop = GetComponentInParent<Loop2>();
        startPosition = cat.transform.position;

        Cat.OnCatJumpedOnBoat += OnCatJumpedOnBoat;
    }

    private void OnCatJumpedOnBoat()
    {
        StopCoroutine(jumpCatCoroutine);
        jumpCatSequence.Kill();
        Destroy(cat);
        FindFirstObjectByType<Gondola>().catOnBoat.SetActive(true);

        loop.catEventCompleted = true;
    }

    void Start()
    {
        jumpCatCoroutine = StartCoroutine(PlayCatAnimation());
    }

    private IEnumerator PlayCatAnimation()
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
