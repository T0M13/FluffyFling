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
    [SerializeField] private BirdState targetState;

    public Transform Target { get => target; set => target = value; }
    public BirdState TargetState { get => targetState; set => targetState = value; }

    public void SetTarget(GameObject bird, BirdState state)
    {
        target = bird.transform;
        targetState = state;
    }

    private void Update()
    {
        if (target == null) return;
        if (TargetState == BirdState.Thrown || TargetState == BirdState.Loaded)
        {
            Vector3 targetPos = target.position + offset;
            targetPos.y = transform.position.y; // Clamp the y-axis position

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }


}
