using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    public Transform doorTransform;
    public Transform openDoorTransform;
    public float doorOpeningSpeed = 10f;
    Vector3 closedPos, openPosition;

    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        closedPos = doorTransform.position;
        openPosition = openDoorTransform.position;
        targetPosition = closedPos;
    }

    private void Update()
    {
        doorTransform.position = Vector3.Lerp(doorTransform.position, targetPosition, Time.deltaTime * doorOpeningSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            targetPosition = openPosition;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            targetPosition = closedPos;

    }
}
