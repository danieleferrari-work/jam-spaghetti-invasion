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

    Vector3 startPosition;

    void Awake()
    {
        startPosition = cat.transform.position;
    }

    void Start()
    {
        StartCoroutine(PlayCatAnimation());
    }

    private IEnumerator PlayCatAnimation()
    {
        while (true)
        {
            cat.gameObject.SetActive(true);
            cat.transform.position = startPosition;
            cat.transform.DOJump(catJumpEndPosition.transform.position, jumpForce, 1, jumpDuration);
            yield return new WaitForSeconds(jumpDuration);
            cat.gameObject.SetActive(false);
            yield return new WaitForSeconds(jumpDelay);
        }
    }
}
