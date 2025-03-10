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

    public void DoJumpOnBoat(GameObject pointOnBoat)
    {
        catRealRoot.transform.SetParent(pointOnBoat.transform, true);
        animator.SetBool("IsJumpingOnBoat", true);
        animator.SetTrigger("DoJump");
    }

    public void DoJump()
    {
        animator.SetTrigger("DoJump");
    }

    public void OnJumpEnd()
    {
        if (animator.GetBool("IsJumpingOnBoat"))
        {
            Debug.Log("jump on boat");
            OnJumpOnBoatFinished?.Invoke();
            animator.applyRootMotion = false;
            Vector3 catTargetPosition = catMovingRoot.transform.position;
            catMovingRoot.transform.localPosition = Vector3.zero;
            catRealRoot.transform.position = catTargetPosition;
        }
    }

    public void OnCatIdleStart()
    {
        // animator.applyRootMotion = false;
        // Vector3 catTargetPosition = catMovingRoot.transform.position;
        // catMovingRoot.transform.localPosition = Vector3.zero;
        // catRealRoot.transform.position = catTargetPosition;
    }
}
