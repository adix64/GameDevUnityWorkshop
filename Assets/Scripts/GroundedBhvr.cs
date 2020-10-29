﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBhvr : StateMachineBehaviour
{
    public float blendSpeed = 10f; // viteza cu care trece aim mode <--> normal mode
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float weaponLayerWeight = animator.GetLayerWeight(1); // layer 0 e Base Layer, layer e HoldWeaponLYR
        float blendFactor;

        if (Input.GetButton("Fire2")) // tinteste activand influenta layer-ului HoldWeaponLYR din animator
            blendFactor = Mathf.Lerp(weaponLayerWeight, 1, Time.deltaTime * blendSpeed);
        else ///  ......... dezactivand ............... daca nu tinteste:
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
        if (animator.GetLayerWeight(1) > 0.5f) // tinteste
        {
            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward,
                                                          Camera.main.transform.right); //rotatia mainii drepte (armei) aliniata la camera
            animator.SetIKRotation(AvatarIKGoal.RightHand, rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f); // jumatate din IK driving
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);  // mana stanga full IK pentru pozitionare pe maner arma
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0.5f); // hint pentru cotul stang orientat corect
            Transform rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            Vector3 weaponHandle = new Vector3(animator.GetFloat("weaponHandleX"), // pozitia manerului armei
                                               animator.GetFloat("weaponHandleY"),
                                               animator.GetFloat("weaponHandleZ"));
            Vector3 weaponElbowHint =  new Vector3(animator.GetFloat("weaponElbowHintX"), //hint pozitia cotului
                                                   animator.GetFloat("weaponElbowHintY"),
                                                   animator.GetFloat("weaponElbowHintZ"));
            //setare pozitie mana stanga:
            animator.SetIKPosition(AvatarIKGoal.LeftHand, weaponHandle);
            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, weaponElbowHint);
        }
        else
        {// scoate IK cand nu se tinteste
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
        }
    }
}