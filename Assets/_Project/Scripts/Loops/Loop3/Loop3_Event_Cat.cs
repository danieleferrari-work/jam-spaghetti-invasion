using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Loop3_Event_Cat : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDuration;

    [Header("References")]
    [SerializeField] GameObject catOnGround;
    [SerializeField] GameObject catShadowOnGround;
    [SerializeField] GondolaAutoPilotArea autoPilotArea;

    // References
    Loop3 loop;
    GameObject catOnBoat;


    void Awake()
    {
        loop = GetComponentInParent<Loop3>();
        catOnBoat = FindObjectOfType<Gondola>().catOnBoat;
    }

    void Start()
    {
        autoPilotArea.OnEndMoving += OnAutoPilotFinish;
    }

    private void OnAutoPilotFinish()
    {
        StartCoroutine(PlayCatAnimation());
    }

    IEnumerator PlayCatAnimation()
    {
        yield return new WaitForSeconds(2);

        catShadowOnGround.gameObject.SetActive(false);
        catOnBoat.transform.DOJump(catOnGround.transform.position, jumpForce, 1, jumpDuration);

        yield return new WaitForSeconds(jumpDuration);

        Destroy(catOnBoat.gameObject);
        catOnGround.gameObject.SetActive(true);
        loop.catEventCompleted = true;
    }
}
