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
    Vector3 moveDir;
    [Range(0f, 2f)]
    public float intersectionThreshold = 0.001f;
    Animator animator;
    public float averageVelocity = 0f;
    public float velocityBlendSpeed = 10f;
    Transform headTransform;
    public Transform enemyContainer;
    public List<Transform> enemies; 

    // Apelata o singura data, la initializare
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        cameraTransform = Camera.main.transform;
        initPos = transform.position;
        headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
        Cursor.lockState = CursorLockMode.Locked;
        enemies = new List<Transform>();

        for (int i = 0; i < enemyContainer.childCount; i++)
            enemies.Add(enemyContainer.GetChild(i));

        Time.timeScale = 1f;
    }

    void Update() //apelata de N ori pe secunda, preferabil N > 60FPS, in general N fluctuant
    {
        moveDir = GetMovementAxes();

        ApplyRootMotion();

        ApplyRootRotation();

        HandleJump();

        HandleFallenOffPlatform();

        UpdateAnimatorParams();

        HandleAttack();


    }
    private void LateUpdate()
    {
        if (animator.GetBool("aiming"))
        {
            //se uita la 50 metri inainte
            headTransform.LookAt(cameraTransform.position + cameraTransform.forward * 50f);
        }

    }
    private void HandleAttack()
    {

        if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2"))
            animator.SetTrigger("attack");
    }

    private void UpdateAnimatorParams()
    {
        //trece din spatiu lume in spatiul personaj
        Vector3 characterSpaceDir = transform.InverseTransformDirection(moveDir);

        if (Input.GetKey(KeyCode.LeftShift))
        {// walk speed
            characterSpaceDir *= 0.5f;
        }
        animator.SetFloat("forward", characterSpaceDir.z, 0.2f, Time.deltaTime);
        animator.SetFloat("right", characterSpaceDir.x, 0.2f, Time.deltaTime);
        animator.SetFloat("timeSinceTakeoff", animator.GetFloat("timeSinceTakeoff") + Time.deltaTime);
    }
    private void ApplyRootRotation()
    {
        var stateNfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateNfo.IsTag("attack"))
            return;

        if (animator.GetBool("jump"))
            return;
        Vector3 D = ComputeLookDirection();
        Vector3 F = transform.forward;
        Vector3 FplusD = F + D;
        Vector3 FminusD = F - D;

        if (FplusD.magnitude > 0.001f && FminusD.magnitude > 0.001f)
        {//rotim cu unghiul dat de dot product in jurul axei cross product ca sa alinem 2 directii
            float u = Mathf.Acos(Vector3.Dot(D, F));
            u *= Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(F, D);
            transform.rotation = Quaternion.AngleAxis(u * .25f, axis) * transform.rotation;
        }

        if (FminusD.magnitude < 0.001)
        {
            transform.rotation = Quaternion.AngleAxis(2f, transform.up) * transform.rotation;
        }

       
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private Vector3 ComputeLookDirection()
    {
        Vector3 D = moveDir;

        if (animator.GetBool("aiming")) //daca tinteste
        {
            D = cameraTransform.forward;
            D.y = 0f;
            D = D.normalized;
        }

        float minDist = 999999f;
        int closestEnemyIndex = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemies[i].position);
            if (distToEnemy < minDist && distToEnemy < 4f)
            {
                minDist = distToEnemy;
                closestEnemyIndex = i;
            }
        }

        animator.SetFloat("distToClosestEnemy", minDist);

        if (closestEnemyIndex != -1)
        {
            D = (enemies[closestEnemyIndex].position - transform.position);
            D.y = 0;
            D = D.normalized;
        }


        return D;
    }

    private void HandleFallenOffPlatform()
    {
        if (transform.position.y < minYthreshold)
            animator.SetInteger("HP", -1);
    }

    private void HandleJump()
    {
        // raza incepe de la baza talpilor, se duce in jos
        Ray ray = new Ray(transform.position + Vector3.up * intersectionThreshold, Vector3.down);
        float maxDist = 2f * intersectionThreshold; //foarte aproape de sol
        if (Physics.Raycast(ray, maxDist))
        {// daca are loc intersectia, e pe sol
            animator.SetBool("jump", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {// sari
                Vector3 jumpVelocity = (Vector3.up + moveDir) * jumpPower;
                //setam directia in care sare pentru a fi folosita in state machine behaviour
                animator.SetFloat("jumpDirX", jumpVelocity.x);
                animator.SetFloat("jumpDirY", jumpVelocity.y);
                animator.SetFloat("jumpDirZ", jumpVelocity.z);
                animator.SetTrigger("jumpTakeoff");//declanseaza decolarea
            }
        }
        else
        {
            if(averageVelocity < 10e-5f) //epsilon
                animator.SetBool("jump", false); // sta pe loc
            else
                animator.SetBool("jump", true); // midair
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

    private void ApplyRootMotion()
    {
        averageVelocity = Mathf.Lerp(averageVelocity, rigidbody.velocity.magnitude, Time.deltaTime * velocityBlendSpeed);
        if (animator.GetBool("jump"))
        { // nu rotim in timp ce sare
            animator.applyRootMotion = false; // nu imprima root motion in aer
            return;
        }
        animator.applyRootMotion = true;
        // pentru suprascrierea directa a pozitiei, daca nu aveam Rigidbody:
            //transform.position += moveDir * Time.fixedDeltaTime * speed;
        float velY = rigidbody.velocity.y; // retinem viteza pe axa verticala
        //rigidbody.velocity = moveDir * speed; // ii dam viteza in functie de directia deplasarii
        rigidbody.velocity = animator.deltaPosition / Time.deltaTime;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY, //pastram viteza pe axa verticala
                                         rigidbody.velocity.z);

    }
}
