using UnityEngine;
using UnityEngine.Events;

public class CatAnimationController : MonoBehaviour
{
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


    public void JumpOnBoatFinished()
    {
        OnJumpOnBoatFinished?.Invoke();
    }

    public void ExitFinished()
    {
        OnJumpOnBoatFinished?.Invoke();
    }
}
