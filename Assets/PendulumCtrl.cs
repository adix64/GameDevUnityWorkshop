using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumCtrl : MonoBehaviour
{
    public float freq = 1f;
    public float amplitude = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * freq) * amplitude);
    }
}
