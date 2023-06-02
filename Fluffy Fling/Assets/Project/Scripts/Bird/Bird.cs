using System;
using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private SphereCollider coll;
    [SerializeField] private Slingshot parent;
    [SerializeField] private Animator animator;
    [SerializeField] private int animationState;
    [SerializeField] private int[] animationMaxStates;
    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private float fallThresshold = -10f;
    [SerializeField] private bool hasCollided;
    [SerializeField] private bool abilityActivated;
    [SerializeField] private bool isDead;
    [SerializeField] private float timerTillDeath = 3f;
    [SerializeField] private BirdState state;
    [SerializeField] private Vector3 colliderOffset = new Vector3(-0.25f, 0, 0);

    public BirdState State { get => state; set => state = value; }
    public Rigidbody Body { get => body; set => body = value; }
    public Slingshot Parent { get => parent; set => parent = value; }


    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += ActivateAbility;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= ActivateAbility;
    }

    private void Start()
    {
        Body = GetComponent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
        animator = GetComponentInChildren<Animator>();
        Body.isKinematic = true;
        abilityActivated = false;
        isDead = false;
        State = BirdState.BeforeThrown;
        SetAnimation(GetRandomAnimation());
    }

    private void Update()
    {
        if ((state == BirdState.Thrown && body.velocity == Vector3.zero) && !isDead || (transform.position.y < fallThresshold) && !isDead)
        {
            Disappear();
        }
        if (state == BirdState.Thrown && hasCollided && body.velocity != Vector3.zero && !isDead)
        {
            timerTillDeath -= Time.deltaTime;
            if (timerTillDeath <= 0)
                Disappear();
        }
    }

    private void OnValidate()
    {
        if (Body == null)
            Body = GetComponent<Rigidbody>();
        if (coll == null)
            coll = GetComponent<SphereCollider>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator != null)
            SetAnimation(animationState);
    }

    private void Disappear()
    {
        state = BirdState.Dead;
        isDead = true;
        StartCoroutine(Die(deathDelay));
    }

    private IEnumerator Die(float deathDelay)
    {
        GameManager.instance.OnDeath?.Invoke(gameObject);
        parent.LastThrowBird = null;
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

    private int GetRandomAnimation()
    {
        int random = UnityEngine.Random.Range(1, animationMaxStates.Length);
        return random;
    }

    private void SetAnimation(int i)
    {
        animationState = i;
        animator.SetInteger("animation", animationState);
    }

    public virtual void ActivateAbility(Vector2 _)
    {
        if (state != BirdState.Thrown) return;
        if (abilityActivated) return;
        if (hasCollided) return;
        Debug.Log("Activating Ability");
        abilityActivated = true;
    }

    public void OnLoaded()
    {
        State = BirdState.Loaded;
        SetAnimation(0);
        //GameManager.instance.OnLoaded?.Invoke(gameObject, State);
        //parent.CameraAddTarget(gameObject, State);
    }

    public void OnThrow()
    {
        Body.isKinematic = false;
        State = BirdState.Thrown;
        SetAnimation(14); //Fly Animation
        //GameManager.instance.OnThrow?.Invoke(gameObject, State);
        //parent.CameraAddTarget(gameObject, State);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasCollided = true;
        coll.center = colliderOffset;
        animationState = 9; //Die Animation
        animator.SetInteger("animation", animationState);
    }


}
