using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ToFollow;
    public float Speed = 1f;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - ToFollow.transform.position;
    }

    void LateUpdate()
    {
        if (ToFollow == null) return;

        var targetPos = ToFollow.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * Speed);
    }
}
