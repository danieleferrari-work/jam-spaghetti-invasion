using UnityEngine;

public class CatJumpStateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Loop3_Event_Cat>().CatJumpFinished();
    }
}
