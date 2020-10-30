using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrooshairCtrl : MonoBehaviour
{
    UnityEngine.UI.Image image;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("aiming"))
            image.enabled = true;
        else
            image.enabled = false;
    }
}
