using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    public float projectileSpeed = 50f;
    Transform cameraTransform;
    float projectileTime = 0f;
    const float convergenceSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        StartCoroutine(AutodestroyProjectile(5f));
    }
    IEnumerator AutodestroyProjectile(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 projectileDir = Vector3.Lerp(transform.forward,
                                             cameraTransform.forward,
                                             Mathf.Clamp01(projectileTime * convergenceSpeed));
        transform.position += projectileDir * Time.deltaTime * projectileSpeed;
        transform.LookAt(transform.position + projectileDir);
        projectileTime += Time.deltaTime;
    }
}
