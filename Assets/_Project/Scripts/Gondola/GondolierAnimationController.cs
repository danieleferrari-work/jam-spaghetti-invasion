using UnityEngine;

public class GondolierAnimationController : MonoBehaviour
{
    [SerializeField] float movingFadeOutDuration;
    [SerializeField] float movingFadeInDuration;
    [SerializeField] AudioSource3D rowingSound;
    [SerializeField] AudioSource3D movingSound;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GondolaMovementManager.OnDoRowing += OnDoRowing;
        GondolaMovementManager.OnStartMoving += OnStartMoving;
        GondolaMovementManager.OnStopMoving += OnStopMoving;
    }

    // Used by animation event
    public void OnRowTouchesWater()
    {
        rowingSound.Play();
    }

    // Used by animation event
    public void OnRowAnimationEnds()
    {
        animator.SetBool("IsRowing", false);
    }


    void OnDoRowing()
    {
        animator.SetBool("IsRowing", true);
    }

    void OnStopMoving()
    {
        movingSound.Stop();
    }

    void OnStartMoving()
    {
        movingSound.Play();
    }
}
