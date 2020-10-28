using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBhvr : StateMachineBehaviour
{
    public float blendSpeed = 10f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float weaponLayerWeight = animator.GetLayerWeight(1);
        float blendFactor;

        if (Input.GetButton("Fire2"))
            blendFactor = Mathf.Lerp(weaponLayerWeight, 1, Time.deltaTime * blendSpeed);
        else
            blendFactor = Mathf.Lerp(weaponLayerWeight, 0, Time.deltaTime * blendSpeed); 

        animator.SetLayerWeight(1, blendFactor);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(1, 0f);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
