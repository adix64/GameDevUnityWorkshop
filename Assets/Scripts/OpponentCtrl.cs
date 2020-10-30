using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCtrl : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    Animator animator;
    Rigidbody rigidbody;
    Vector3 moveDir;
    public float attackDistanceThreshold = 1.1f; // distanta minima de la care ataca oponentul
    public float attackDistFreq = 1f;
    public float intersectionThreshold = 0.15f;
    float deadTime = 0f;
    int aiTargetMode = 0; // 0 -- fixeaza personajul, 1 -- ocol prin dreapta, 2 -- ocol prin stanga, 3 -- retragere

    SkinnedMeshRenderer skinnedMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(SeedAIdirection(1f));
    }

    IEnumerator SeedAIdirection(float t)
    { // o data la 0.5 - 3 secunde, se schimba comportamentul AIului
        yield return new WaitForSeconds(t);
        aiTargetMode = UnityEngine.Random.Range(0, 4);
        float waitTime = UnityEngine.Random.Range(.5f, 3f);
        StartCoroutine(SeedAIdirection(waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (!VerifyAlive())
            return;
        SetAgentDestination();
        ApplyRootRotation();
        UpdateAnimatorParams();
        HandleAttack();
        HandleMidair();
    }
    bool VerifyAlive()
    {
        var stateNfo = animator.GetCurrentAnimatorStateInfo(0);
        bool alive = !stateNfo.IsName("Dead");
        if (!alive)
        {
      
            agent.SetDestination(transform.position);
            deadTime += Time.deltaTime;
        }

        if (deadTime > 5f)
            if (!skinnedMeshRenderer.isVisible)
            { // eliberam memoria eliminand oponenti decedati cand acestia nu sunt vizibili
                player.GetComponent<MovePlayer>().enemies.Remove(transform);
                Destroy(gameObject);
            }
        return alive;
    }

    private void HandleMidair()
    {
        // raza incepe de la baza talpilor, se duce in jos
        Ray ray = new Ray(transform.position + Vector3.up * intersectionThreshold, Vector3.down);
        float maxDist = 2f * intersectionThreshold; //foarte aproape de sol
        if (Physics.Raycast(ray, maxDist))
        {// daca are loc intersectia, e pe sol
            animator.SetBool("jump", false);
            agent.speed = 0.5f;
        }
        else
        {
            agent.speed = 5f;
            agent.velocity = rigidbody.velocity;
            animator.SetBool("jump", true);
        }
    }

    private void SetAgentDestination()
    {
        Vector3 offset = Vector3.zero;
        switch (aiTargetMode)
        {
            case 0: // fixeaza playerul
                offset = Vector3.zero;
                break;
            case 1: // ocol prin dreapta
                offset = transform.right;
                break;
            case 2: // ocol prin stanga
                offset = -transform.right;
                break;
            case 3: // retragere
                offset = -transform.forward * UnityEngine.Random.Range(2f, 4f);
                break;
        }

        agent.SetDestination(player.position + offset); // pozitie destinatie pentru NAV mesh agent, deplaseaza AI la jucator automat
    }

    private void HandleAttack()
    {
        attackDistanceThreshold = (Mathf.Sin(Time.time * attackDistFreq) + 1f) * 0.5f + 0.5f; // oscileaza intre 0.5 - 1.5
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
        Vector3 D = player.position - transform.position;
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
