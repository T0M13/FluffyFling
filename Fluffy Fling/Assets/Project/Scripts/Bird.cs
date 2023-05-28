using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private BirdState state;

    public BirdState State { get => state; set => state = value; }

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        State = BirdState.BeforeThrown;
    }

    public void OnThrow()
    {
        body.isKinematic = false;
        State = BirdState.Thrown;
    }


}
