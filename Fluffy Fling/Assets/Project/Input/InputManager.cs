using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position);
    public event EndTouch OnEndTouch;
    #endregion

    public static InputManager instance { get; private set; }

    public Slingshot swipeDetector;

    private Controls playerControls;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        swipeDetector = GetComponent<Slingshot>();
        playerControls = new Controls();
        mainCamera = Camera.main;
    }

    private void OnValidate()
    {
        swipeDetector = GetComponent<Slingshot>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
            if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));
    }


    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
            if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()));
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }


}
