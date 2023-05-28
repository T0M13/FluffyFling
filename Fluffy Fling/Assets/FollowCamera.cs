using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float smoothTime = .25f;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    public Transform Target { get => target; set => target = value; }

    private void Update()
    {
        if (target == null) return;
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

}
