using UnityEngine;

public class GondolierAnimationController : MonoBehaviour
{
    [SerializeField] AudioSource3D rowingSound;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GondolaMovementManager.OnStartRowing += OnStartRowing;
        GondolaMovementManager.OnStopRowing += OnStopRowing;
    }

    // Used by animation event
    public void OnRowTouchesWater()
    {
        rowingSound.Play();
    }


    private void OnStartRowing()
    {
        animator.SetBool("IsRowing", true);
    }

    private void OnStopRowing()
    {
         animator.SetBool("IsRowing", false);
    }
}
