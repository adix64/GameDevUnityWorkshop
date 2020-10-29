using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateCtrl : MonoBehaviour
{
    public float vertFreq = 1f;
    public float horizFreq = 1f;
    public float vertAmplitude = 1f;
    public float horizAmplitude = 1f;

    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos +
                             transform.up * Mathf.Sin(Time.time * vertFreq) * vertAmplitude +
                             transform.right * Mathf.Sin(Time.time * horizFreq) * horizAmplitude;
    }
}
