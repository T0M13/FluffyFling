using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private BirdState state;

    public BirdState State { get => state; set => state = value; }
    public Rigidbody Body { get => body; set => body = value; }

    private void Start()
    {
        Body = GetComponent<Rigidbody>();
        Body.isKinematic = true;
        State = BirdState.BeforeThrown;
    }

    public void OnThrow()
    {
        Body.isKinematic = false;
        State = BirdState.Thrown;
    }


}
