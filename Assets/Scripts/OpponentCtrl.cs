using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCtrl : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    Animator animator;
    Vector3 moveDir;
    public float attackDistanceThreshold = 1.1f; // distanta minima de la care ataca oponentul
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();        
        animator = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position); // pozitie destinatie pentru NAV mesh agent, deplaseaza AI la jucator automat
        ApplyRootRotation();
        UpdateAnimatorParams();
        HandleAttack();
    }
    private void HandleAttack()
    {
        if (agent.remainingDistance < attackDistanceThreshold)
        {
            animator.SetTrigger("attack");
        }
    }

    private void UpdateAnimatorParams()
    {
        moveDir = agent.velocity.normalized;
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
        Vector3 D = moveDir;
        D.y = 0f;
        D = D.normalized;

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
    }
}
