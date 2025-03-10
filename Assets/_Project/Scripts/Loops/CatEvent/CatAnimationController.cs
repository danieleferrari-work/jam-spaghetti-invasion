using UnityEngine;
using UnityEngine.Events;

public class CatAnimationController : MonoBehaviour
{
    [SerializeField] GameObject catRealRoot;
    [SerializeField] GameObject catMovingRoot;

    Animator animator;

    public UnityAction OnJumpOnBoatFinished;


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

    public void OnJumpOnBoatEnd()
    {
        OnJumpOnBoatFinished?.Invoke();
        
        Debug.Log("jump end");
        animator.applyRootMotion = false;
        Vector3 catTargetPosition = catMovingRoot.transform.position;
        catMovingRoot.transform.localPosition = Vector3.zero;
        catRealRoot.transform.position = catTargetPosition;
    }
}
