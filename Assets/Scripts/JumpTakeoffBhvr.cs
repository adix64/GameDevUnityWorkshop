﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTakeoffBhvr : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float timer = animator.GetFloat("timeSinceTakeoff");
        float normalizedTime = stateInfo.normalizedTime;
        if (normalizedTime > 0.64f && timer > 0.5f)
        {//propulseaza de pe sol personajul in sus daca s-a trecut de 64% din clipul takeoff
            animator.SetFloat("timeSinceTakeoff", 0f);
            Vector3 jumpVelocity = new Vector3(animator.GetFloat("jumpDirX"),
                                              animator.GetFloat("jumpDirY"),
                                              animator.GetFloat("jumpDirZ"));
            animator.transform.GetComponent<Rigidbody>().AddForce(jumpVelocity, ForceMode.VelocityChange);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
