using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldWeaponScript : MonoBehaviour
{
    Animator animator;
    public Transform weapon;
    public Transform weaponTip;
    public Transform weaponHandle;
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
            animator.SetBool("aiming", true);
            weapon.gameObject.SetActive(true); // face arma vizibila
            //pune arma in liniile personajului
            weapon.transform.position = rightHand.position;
            weapon.transform.rotation = rightHand.rotation;
            //salvam in animator pozitia manerului armei
            animator.SetFloat("weaponHandleX", weaponHandle.position.x);
            animator.SetFloat("weaponHandleY", weaponHandle.position.y);
            animator.SetFloat("weaponHandleZ", weaponHandle.position.z);

            if (Input.GetButtonDown("Fire1"))
            {// se instantiaza proiectil, SHOOT
                GameObject projectileGO = GameObject.Instantiate(projectile);
                projectileGO.transform.position = weaponTip.position;
                projectileGO.transform.rotation = weaponTip.rotation;
            }
        }
        else
        {
            animator.SetBool("aiming", false);
            weapon.gameObject.SetActive(false); // face arma invizibila
        }
    }
}
