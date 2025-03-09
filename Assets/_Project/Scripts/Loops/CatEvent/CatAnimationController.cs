using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    [SerializeField] GameObject catRealRoot;
    [SerializeField] GameObject catMovingRoot;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnJumpOnBoatEnd()
    {
        Debug.Log("jump end");
        animator.applyRootMotion = false;
        Vector3 catTargetPosition = catMovingRoot.transform.position;
        catMovingRoot.transform.localPosition = Vector3.zero;
        catRealRoot.transform.position = catTargetPosition;
    }
}
