using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int life = 1;
    public float speed = 1.0f; // rotation speed for each tap
    public float duration = 1.0f; // base time for whole 360 degrees
    public float durationScaler = 1.1f; 

    public int rotationCount = 0;

    private bool isRotating = false;
    public float rotationDuration = 1.0f;
    public int maxRotationCount = 20;

    // Use this for initialization
    void Start()
    {
        isRotating = false;
        rotationDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DownEvent()
    {
        if (maxRotationCount > rotationCount)
            rotationDuration /= durationScaler;
        rotationCount++;
        if (!isRotating)
        {
            rotationDuration = duration;
            EventManager.TriggerEvent("startMoving");
            isRotating = true;
            StartCoroutine(Rotate());
        }

    }

    IEnumerator Rotate()
    {
        Quaternion startRot = transform.rotation;
        while (rotationCount > 0)
        {
            float t = 0.0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                transform.rotation = startRot * Quaternion.AngleAxis(t / rotationDuration * 360f, Vector3.forward); //or transform.right if you want it to be locally based
                yield return null;
            }

            rotationCount--;
            if (maxRotationCount > rotationCount)
                rotationDuration *= durationScaler;
        }

        rotationDuration = duration;
        transform.rotation = startRot;
        isRotating = false;
    }
}
