using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 3f;
    Vector3 initPos;
    Transform cameraTransform;
    public float minYthreshold = -50f;
    public float jumpPower = 2f;
    CapsuleCollider capsule;
    Rigidbody rigidbody;
    const float intersectionThreshold = 0.001f;

    // Apelata o singura data, la initializare
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        cameraTransform = Camera.main.transform;
        initPos = transform.position;
    }

    void Update() //apelata de N ori pe secunda, preferabil N > 60FPS, in general N fluctuant
    {
        Vector3 moveDir = GetMovementAxes();

        ApplyRootMotion(moveDir);

        HandleJump();

        HandleFallenOffPlatform();
    }

    private void HandleFallenOffPlatform()
    {
        if (transform.position.y < minYthreshold)
            rigidbody.position = initPos;
    }

    private void HandleJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down); // raza incepe in centrul capsulei, se duce in jos
        float maxDist = capsule.height * 0.5f + intersectionThreshold; //foarte aproape de sol
        if (Physics.Raycast(ray, maxDist))
        {// daca are loc intersectia
            if (Input.GetKeyDown(KeyCode.Space))
            {// sari
                Vector3 jumpVec = Vector3.up * jumpPower;
                rigidbody.AddForce(jumpVec, ForceMode.VelocityChange);
            }
        }
    }
    private Vector3 GetMovementAxes()
    {
        float x = Input.GetAxis("Horizontal"); //-1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float z = Input.GetAxis("Vertical"); //-1 pentru tasta S, 1 pentru tasta W, 0 altfel

        //obtinem directia de deplasare relativ la camera:
        Vector3 moveDir = x * cameraTransform.right + z * cameraTransform.forward;
        moveDir.y = 0f; // blocam translatia pe verticala(ne desplasam doar in planul xOz)
        moveDir = moveDir.normalized; // normalizand, obtinem vectorul cu lungime unitate(vector directie)
        return moveDir;
    }

    private void ApplyRootMotion(Vector3 moveDir)
    {
        // pentru suprascrierea directa a pozitiei, daca nu aveam Rigidbody:
            //transform.position += moveDir * Time.fixedDeltaTime * speed;
        float velY = rigidbody.velocity.y; // retinem viteza pe axa verticala
        rigidbody.velocity = moveDir * speed; // ii dam viteza in functie de directia deplasarii
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY, //pastram viteza pe axa verticala
                                         rigidbody.velocity.z); 
    }
}
