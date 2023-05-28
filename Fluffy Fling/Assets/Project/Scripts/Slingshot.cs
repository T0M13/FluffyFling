using System;
using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private InputManager inputManager;

    [Header("Finger Settings")]
    [SerializeField] private float radiusThreshhold = 1.2f;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private Vector2 endPosition;
    private Camera mainCamera;
    [SerializeField] private LineRenderer trajectory;

    [Header("Slingshot Settings")]
    [SerializeField] private Transform birdRestPosition;
    [SerializeField] private GameObject bird;
    [SerializeField] private float throwSpeed = 7f;
    [SerializeField] private float throwThreshhold = 0.5f;
    [SerializeField] private float pullThreshhold = 2.5f;
    [SerializeField] private bool isPulling;
    [SerializeField] private SlingshotState state;
    [SerializeField] private Vector3 birdInitialPosition;
    [SerializeField] private Vector3 pullStartPosition;

    public SlingshotState State { get => state; set => state = value; }

    private void Awake()
    {
        mainCamera = Camera.main;
        inputManager = InputManager.instance;
        trajectory = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        currentPosition = Vector2.zero;
        state = SlingshotState.Idle;
        birdInitialPosition = bird.transform.position;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position)
    {
        startPosition = position;
        InitializeThrow();
        if (IsBirdClicked())
        {
            pullStartPosition = startPosition;
            state = SlingshotState.Pulling;
            isPulling = true;
        }
    }

    private void SwipeEnd(Vector2 position)
    {
        isPulling = false;
        endPosition = position;
    }


    private void Update()
    {
        currentPosition = inputManager.PrimaryPosition();

        PullBird();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, radiusThreshhold);
        Gizmos.DrawLine(startPosition, currentPosition);
    }

    private bool IsBirdClicked()
    {
        Collider[] hitObject = Physics.OverlapSphere(startPosition, radiusThreshhold);
        if (hitObject.Length <= 0) return false;
        else
        {
            if (hitObject[0].gameObject == bird)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void InitializeThrow()
    {
        if (bird == null) return;
        bird.transform.position = birdRestPosition.position;
        birdInitialPosition = bird.transform.position;
        state = SlingshotState.Idle;
    }

    private void PullBird()
    {
        if (bird == null) return;
        if (isPulling)
        {
            Vector3 pullPosition = currentPosition;
            Vector3 pullDirection = pullPosition - pullStartPosition;
            float pullDistance = Mathf.Clamp(pullDirection.magnitude, 0f, pullThreshhold);

            bird.transform.position = birdInitialPosition + pullDirection.normalized * pullDistance;

            float pullTrajectory = Vector3.Distance(birdInitialPosition, bird.transform.position);

            float distance = Vector3.Distance(bird.transform.position, birdInitialPosition);
            if (distance > throwThreshhold)
                ShowTrajectory(pullTrajectory);
            else
                SetTrajectoryActive(false);

        }
        else
        {
            SetTrajectoryActive(false);
            float distance = Vector3.Distance(bird.transform.position, birdInitialPosition);
            if (distance > throwThreshhold)
            {
                state = SlingshotState.Flying;
                ThrowBird(distance);
            }
            else
            {
                bird.transform.position = birdInitialPosition;
            }
        }

    }

    private void ThrowBird(float distance)
    {
        Vector3 velocity = (birdInitialPosition - bird.transform.position).normalized;
        bird.GetComponent<Bird>().OnThrow();
        bird.GetComponent<Rigidbody>().velocity = velocity * throwSpeed * distance;
        bird.GetComponent<Rigidbody>().useGravity = true;
        bird = null;
        state = SlingshotState.Idle;

    }

    private void SetTrajectoryActive(bool active)
    {
        trajectory.enabled = active;
    }

    private void ShowTrajectory(float distance)
    {
        SetTrajectoryActive(true);
        Vector3 diff = birdInitialPosition - bird.transform.position;
        int segmentCount = 25;
        Vector2[] segments = new Vector2[segmentCount];
        segments[0] = bird.transform.position;

        Vector2 segVelocity = diff.normalized * throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float timeCurve = (i * Time.fixedDeltaTime * 5);
            segments[i] = segments[0] + segVelocity * timeCurve + 0.5f * (Vector2)Physics.gravity * (timeCurve * timeCurve);
        }

        trajectory.positionCount = segmentCount;
        for (int j = 0; j < segmentCount; j++)
        {
            trajectory.SetPosition(j, segments[j]);
        }
    }




}
