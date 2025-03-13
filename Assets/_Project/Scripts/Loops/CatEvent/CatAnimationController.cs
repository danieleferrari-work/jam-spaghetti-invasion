using UnityEngine;
using UnityEngine.Events;

public class CatAnimationController : MonoBehaviour
{
    [SerializeField] AudioSource3D catAudioSplash;
    [SerializeField] AudioSource3D catAudioMeowing;
    
    Animator animator;

    public UnityAction OnJumpOnBoatFinished;
    public UnityAction OnExitFinished;



    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DoJumpOnBoat()
    {
        animator.SetTrigger("DoJumpOnBoat");
    }

    public void DoJump()
    {
        animator.SetTrigger("DoJump");
    }

    public void DoExit()
    {
        animator.SetTrigger("DoExit");
    }

    public void JumpOnBoatStarted()
    {
        catAudioMeowing.Play();
    }

    public void JumpOnBoatFinished()
    {
        OnJumpOnBoatFinished?.Invoke();
    }

    public void ExitFinished()
    {
        OnJumpOnBoatFinished?.Invoke();
    }

    public void JumpFinished()
    {
        catAudioSplash.Play();
    }
}
