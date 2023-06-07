using System;
using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("References")]
    private InputManager inputManager;
    private Camera mainCamera;
    private FollowCamera followCamera;
    [SerializeField] private LineRenderer trajectory;
    [SerializeField] private LineRenderer lastBirdTrail;
    [SerializeField] private Transform leftRubberOrigin, rightRubberOrigin;
    [SerializeField] private LineRenderer leftRubber, rightRubber;
    [SerializeField] private BirdsManager birdsManager;

    [Header("Finger Settings")]
    [SerializeField] private float radiusThreshhold = 1.2f;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private Vector2 endPosition;

    [Header("Slingshot Settings")]
    [SerializeField] private Transform birdRestPosition;
    [SerializeField] private GameObject bird;
    [SerializeField] private GameObject lastThrowBird;
    [SerializeField] private float throwSpeed = 7f;
    [SerializeField] private float throwThreshhold = 0.5f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private float pullThreshhold = 2.5f;
    [SerializeField] private bool isPulling;
    [SerializeField] private SlingshotState state;
    [SerializeField] private Vector3 birdInitialPosition;
    [SerializeField] private Vector3 pullStartPosition;
    [SerializeField] private float lastPullTrajectory;
    [SerializeField] private int trajectoryLength = 25;

    [SerializeField] private float minimumDistanceSwipe = 1;

    public SlingshotState State { get => state; set => state = value; }
    public GameObject LastThrowBird { get => lastThrowBird; set => lastThrowBird = value; }

    private void Awake()
    {
        mainCamera = Camera.main;
        followCamera = mainCamera.GetComponent<FollowCamera>();
        inputManager = InputManager.instance;
        birdsManager = FindObjectOfType<BirdsManager>();
    }

    private void Start()
    {
        currentPosition = Vector2.zero;
        state = SlingshotState.Idle;
        StartCoroutine(TryGetNextBird(0));
        InitializeThrow();
        SetRubber();
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
        if (GameManager.instance.Paused) return;

        startPosition = position;
        if (IsBirdClicked() && lastThrowBird == null)
        {
            followCamera.Follow = true;
            SetRubberActive(true);
            pullStartPosition = startPosition;
            state = SlingshotState.Pulling;
            isPulling = true;
        }

    }

    private void SwipeEnd(Vector2 position)
    {
        if (GameManager.instance.Paused) return;

        endPosition = position;
        if (!IsBirdClicked() && !isPulling && (state == SlingshotState.Idle))
        {
            CameraSwipe();
        }
        isPulling = false;
        state = SlingshotState.Idle;
    }

    private void CameraSwipe()
    {
        // Update the camera position based on the distance between start and end positions
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistanceSwipe)
        {
            followCamera.Follow = false;
            followCamera.ResetTimer();
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            direction.y = 0;
            followCamera.MoveToPosition -= direction;
        }
    }


    private void Update()
    {
        if (GameManager.instance.Paused) return;

        currentPosition = inputManager.PrimaryPosition();

        PullBird();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPosition, radiusThreshhold);
        Gizmos.DrawLine(startPosition, currentPosition);
    }

    private void SetRubber()
    {
        leftRubber.SetPosition(0, leftRubberOrigin.position);
        rightRubber.SetPosition(0, rightRubberOrigin.position);
    }

    private void SetRubberActive(bool active)
    {
        leftRubber.enabled = active;
        rightRubber.enabled = active;
    }

    private void DisplayRubber()
    {
        if (bird == null) return;
        leftRubber.SetPosition(1, bird.transform.position);
        rightRubber.SetPosition(1, bird.transform.position);
    }

    private bool IsBirdClicked()
    {
        Collider[] hitObject = Physics.OverlapSphere(startPosition, radiusThreshhold);
        if (hitObject.Length <= 0) return false;
        if (hitObject.Length > 1) return false;
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
            DisplayRubber();
            Vector3 pullPosition = currentPosition;
            Vector3 pullDirection = pullPosition - pullStartPosition;
            float pullDistance = Mathf.Clamp(pullDirection.magnitude, 0f, pullThreshhold);

            bird.transform.position = birdInitialPosition + pullDirection.normalized * pullDistance;
            state = SlingshotState.Pulling;
            SetCameraTarget(bird, state);
            lastPullTrajectory = Vector3.Distance(birdInitialPosition, bird.transform.position);

            float distance = Vector3.Distance(bird.transform.position, birdInitialPosition);
            if (distance > throwThreshhold)
            {
                SetTrajectoryActive(true);
                CalculateTrajectory(lastPullTrajectory, trajectory);
            }
            else
                SetTrajectoryActive(false);

        }
        else
        {
            SetTrajectoryActive(false);
            state = SlingshotState.Idle;
            if (LastThrowBird == null)
                SetCameraTarget(bird, state);
            if (lastThrowBird != null) return;
            float distance = Vector3.Distance(bird.transform.position, birdInitialPosition);
            if (distance > throwThreshhold)
            {
                state = SlingshotState.Flying;
                SetCameraTarget(bird, state);
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

        if (AudioManager.instance)
            AudioManager.instance.Play("slingshotRelease");

        CalculateTrajectory(lastPullTrajectory, lastBirdTrail);
        SetLastBirdTrailActive(true);
        isPulling = false;
        SetRubberActive(false);
        Vector3 velocity = (birdInitialPosition - bird.transform.position).normalized;
        bird.GetComponent<Bird>().OnThrow();
        bird.GetComponent<Rigidbody>().velocity = velocity * throwSpeed * distance;
        bird.GetComponent<Rigidbody>().useGravity = true;
        birdsManager.RemoveBird(bird);
        LastThrowBird = bird;
        bird = null;
        StartCoroutine(TryGetNextBird(reloadTime));
    }

    public GameObject GetCurrentBird()
    {
        if (bird != null)
            return bird;
        else return null;
    }

    public IEnumerator TryGetNextBird(float time)
    {
        yield return new WaitForSeconds(time);
        if (birdsManager.SpawnedBirds.Count > 0)
        {
            bird = birdsManager.GetFirstBird();
            Bird birdCode = bird.GetComponent<Bird>();
            birdCode.OnLoaded();
            InitializeThrow();
        }
    }

    public void SetCameraTarget(GameObject bird, SlingshotState state)
    {
        followCamera.SetTarget(bird, state);
    }

    private void SetTrajectoryActive(bool active)
    {
        trajectory.enabled = active;
    }

    private void SetLastBirdTrailActive(bool active)
    {
        lastBirdTrail.enabled = active;
    }

    private void CalculateTrajectory(float distance, LineRenderer lineRenderer)
    {

        Vector3 diff = birdInitialPosition - bird.transform.position;
        int segmentCount = trajectoryLength;
        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = bird.transform.position;

        Vector2 segVelocity = diff.normalized * throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float timeCurve = (i * Time.fixedDeltaTime * 5);
            segments[i] = segments[0] + segVelocity * timeCurve + 0.5f * (Vector2)Physics.gravity * (timeCurve * timeCurve);
        }

        lineRenderer.positionCount = segmentCount;
        for (int j = 0; j < segmentCount; j++)
        {
            lineRenderer.SetPosition(j, segments[j]);
        }
    }




}
