using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(RestrictMovement))]
public class Movement : TouchInput, ICheckable
{
    [SerializeField] private ControllerMovement _controllerMovement;

    private RestrictMovement _restrictMovement;
    private Vector2 _touchPosition;

    private void Awake()
    {
        _restrictMovement = GetComponent<RestrictMovement>();
    }

    public override void OnFingerMove(Finger finger)
    {
        if (finger == _movementFinger)
        {
            ETouch.Touch currentTouch = finger.currentTouch;
            float newPositionX = (currentTouch.screenPosition - _touchPosition).normalized.x;
            _controllerMovement.Run(newPositionX);
            _touchPosition = currentTouch.screenPosition;
            _restrictMovement.SetPosition();
        }
    }

    public override void OnFingerUp(Finger finger)
    {
        if (finger == _movementFinger)
        {
            _movementFinger = null;
        }
    }

    public override void OnFingerDown(Finger finger)
    {
        if (_movementFinger == null)
        {
            bool isItem = CheckItems(finger.screenPosition);

            if(isItem == false)
            {
                _movementFinger = finger;
                _touchPosition = finger.screenPosition;
            }
        }
    }

    public bool CheckItems(Vector2 position)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        ItemFall itemFall = null;

        if (hit.collider != null)
        {
            itemFall = hit.transform.gameObject.GetComponent<ItemFall>();
        }

        if(itemFall != null)
        {
            return true;
        }

        return false;
    }
}