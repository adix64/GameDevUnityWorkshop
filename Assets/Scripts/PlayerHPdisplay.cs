using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPdisplay : MonoBehaviour
{
    public Animator animator;
    RectTransform hpFill;
    // Start is called before the first frame update
    void Start()
    {
        hpFill = transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        hpFill.localScale = new Vector3((float)animator.GetInteger("HP") / 100f, 1, 1);
    }
}
