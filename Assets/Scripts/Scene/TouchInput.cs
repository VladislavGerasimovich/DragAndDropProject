using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine;

namespace Scene
{
    public abstract class TouchInputHandler : MonoBehaviour
    {
        protected Finger MovementFinger;
        //что бы проверить работу приложения в Play моде, необходимо в Input Debugger в Options выбрать:
        //Simulate Touch Input From Mouse or Pen
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

        public abstract void OnFingerMove(Finger finger);

        public abstract void OnFingerUp(Finger finger);

        public abstract void OnFingerDown(Finger finger);
    }
}