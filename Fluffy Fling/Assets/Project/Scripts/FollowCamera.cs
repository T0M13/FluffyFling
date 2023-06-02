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
    [SerializeField] private SlingshotState targetState;

    public Transform Target { get => target; set => target = value; }
    public SlingshotState TargetState { get => targetState; set => targetState = value; }

    [SerializeField] private Vector3 generalPosition;
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private float maxXLimit;
    [SerializeField] private float idleAimValue;
    [SerializeField] private float idleOffsetValue;
    [SerializeField] private float aimingAimValue;
    [SerializeField] private float aimingOffsetValue;
    [SerializeField] private float flyingAimValue;
    [SerializeField] private float flyingOffsetValue;

    public void SetTarget(GameObject bird, SlingshotState state)
    {
        target = bird.transform;
        targetState = state;
    }

    private void Update()
    {
        if (target == null) return;

        switch (targetState)
        {
            case SlingshotState.Idle:
                aimPosition.z = idleAimValue;
                offset.x = idleOffsetValue;
                smoothTime = 0.9f;
                break;
            case SlingshotState.Pulling:
                aimPosition.z = aimingAimValue;
                offset.x = aimingOffsetValue;
                smoothTime = 0.9f;
                break;
            case SlingshotState.Flying:
                aimPosition.z = flyingAimValue;
                offset.x = flyingOffsetValue;
                smoothTime = 0.45f;
                break;
            default:
                break;
        }

        generalPosition = target.position + offset + aimPosition;

        generalPosition.y = transform.position.y; // Clamp the y-axis position

        switch (targetState)
        {
            case SlingshotState.Idle:
                generalPosition.x = Mathf.Clamp(generalPosition.x, offset.x, offset.x);
                break;
            case SlingshotState.Pulling:
                generalPosition.x = Mathf.Clamp(generalPosition.x, offset.x, offset.x);
                break;
            case SlingshotState.Flying:
                generalPosition.x = Mathf.Clamp(generalPosition.x, offset.x, maxXLimit);
                break;
            default:
                break;
        }


        transform.position = Vector3.SmoothDamp(transform.position, generalPosition, ref velocity, smoothTime);
    }


}
