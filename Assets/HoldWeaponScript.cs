using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldWeaponScript : MonoBehaviour
{
    Animator animator;
    public Transform weapon;
    public Transform weaponTip;
    Transform rightHand;

    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        var stateNfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButton("Fire2") && stateNfo.IsTag("grounded"))
        {
            weapon.gameObject.SetActive(true);
            weapon.transform.position = rightHand.position;
            weapon.transform.rotation = rightHand.rotation;
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject projectileGO = GameObject.Instantiate(projectile);
                projectileGO.transform.position = weaponTip.position;
                projectileGO.transform.rotation = weaponTip.rotation;
            }
        }
        else
        {
            weapon.gameObject.SetActive(false);
        }
    }
}
