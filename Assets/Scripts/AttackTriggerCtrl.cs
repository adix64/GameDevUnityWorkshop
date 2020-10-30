using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerCtrl : MonoBehaviour
{
    public string compareLayer;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(compareLayer))
        {
            Animator animator = other.gameObject.transform.parent.GetComponent<Animator>();
            animator.SetTrigger("takeHit");
            animator.SetInteger("HP", animator.GetInteger("HP") - damage);
        }
    }
}
