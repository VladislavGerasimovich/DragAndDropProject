using Items;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Scene
{
    [RequireComponent(typeof(RestrictMovement))]
    public class MovingAroundScene : TouchInputHandler
    {
        [SerializeField] private ControllerPosition _controllerMovement;

        private RestrictMovement _restrictMovement;
        private Vector2 _touchPosition;

        private void Awake()
        {
            _restrictMovement = GetComponent<RestrictMovement>();
        }

        public override void OnFingerMove(Finger finger)
        {
            if (finger == MovementFinger)
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
            if (finger == MovementFinger)
            {
                MovementFinger = null;
            }
        }

        public override void OnFingerDown(Finger finger)
        {
            if (MovementFinger == null)
            {
                bool isItem = CheckItems(finger.screenPosition);

                if (isItem == false)
                {
                    MovementFinger = finger;
                    _touchPosition = finger.screenPosition;
                }
            }
        }

        public bool CheckItems(Vector2 position)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            ItemPhysics itemFall = null;

            if (hit.collider != null)
            {
                itemFall = hit.transform.gameObject.GetComponent<ItemPhysics>();
            }

            if (itemFall != null)
            {
                return true;
            }

            return false;
        }
    }
}