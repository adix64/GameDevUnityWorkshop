using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    float yaw = 0f; //unghiul facut de axele camerei cu axa verticala a lumii                  
    float pitch = 0f; //unghiul facut de axele camerei cu axa orizontala a lumii
    public Transform player; // referinta la target (de la 3rd person camera)
    public float distToPlayer = 4f;
    public float minPitch = -45f;
    public float maxPitch = 45f;

    public Vector3 cameraOffset; // deplasamentul camerei relativ la centru
    public Vector3 aimDownSightsCameraOffset; // deplasamentul camerei relativ la centru pentru aim
    public Animator animator;
    public PauseGameCtrl gamePauseCtrl;
    // LateUpdate este apelat intr-un frame dupa ce Update a fost apelat pe toate obiectele
    void LateUpdate()
    {
        if (gamePauseCtrl.gamePaused)
            return;
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        // limitam unghiul cu axa orizontala, ca sa nu se mai dea camera peste cap:
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); 

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        float blendF = animator.GetLayerWeight(1) * (animator.GetBool("aiming") ? 1f : 0f);
        Vector3 finalCamOffset = Vector3.Lerp(cameraOffset, aimDownSightsCameraOffset, blendF);
                            //de la pozitia playerului, ne dam in spate distToPlayer unitati
        transform.position = player.position - transform.forward * distToPlayer
                            + transform.TransformVector(finalCamOffset);//offset exprimat in spatiul camera, pentru over the shoulder look
    
    
    }
}
