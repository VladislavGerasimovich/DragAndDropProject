using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(RestrictMovement))]
public class TouchInput : MonoBehaviour
{
    [SerializeField] private Transform _controllerPosition;

    private RestrictMovement _restrictMovement;
    private Finger _movementFinger;
    private Vector2 _touchPosition;
    private float _speed;

    private void Awake()
    {
        _restrictMovement = GetComponent<RestrictMovement>();
        _speed = 0.1f;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerUp += OnFingerUp;
        ETouch.Touch.onFingerMove += OnFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerUp -= OnFingerUp;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger == _movementFinger)
        {
            ETouch.Touch currentTouch = finger.currentTouch;
            float newPositionX;
            newPositionX = (currentTouch.screenPosition - _touchPosition).normalized.x;
            _controllerPosition.transform.position = 
                new Vector2(_controllerPosition.transform.position.x + newPositionX * _speed,
                _controllerPosition.transform.position.y);
            _touchPosition = currentTouch.screenPosition;
            _restrictMovement.SetPosition();
        }
    }

    private void OnFingerUp(Finger finger)
    {
        if (finger == _movementFinger)
        {
            _movementFinger = null;
        }
    }

    private void OnFingerDown(Finger finger)
    {
        if (_movementFinger == null)
        {
            _movementFinger = finger;
            _touchPosition = finger.screenPosition;
        }
    }
}