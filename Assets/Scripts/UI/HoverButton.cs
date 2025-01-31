using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using UI;
using System.Collections.Generic;
using UnityEngine.Events;
using Scene;

namespace UI
{
    public class HoverButton : MonoBehaviour
    {
        [SerializeField] private ControllerPosition _controllerMovement;
        [SerializeField] private RestrictMovement _restrictMovement;
        [SerializeField] private float _multiplier;

        private Dictionary<Entry, UnityAction<BaseEventData>> _events;
        private bool _isActive;
        private Button _button;
        private bool _inZone;

        private void Start()
        {
            //что бы добавить к кнопке событие Hover я написал следующее:
            _events = new Dictionary<Entry, UnityAction<BaseEventData>>();
            _button = GetComponent<Button>();
            EventTrigger trigger = _button.gameObject.AddComponent<EventTrigger>();
            Entry enterTrigger = EntryFactory.Create(EventTriggerType.PointerEnter, OnPointerEnter);
            trigger.triggers.Add(enterTrigger);
            _events.Add(enterTrigger, OnPointerEnter);
            Entry exitTrigger = EntryFactory.Create(EventTriggerType.PointerExit, OnPointerExit);
            trigger.triggers.Add(exitTrigger);
            _events.Add(exitTrigger, OnPointerExit);
        }

        private void OnDisable()
        {
            foreach (var item in _events)
            {
                item.Key.callback.RemoveListener(item.Value);
            }
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void OnPointerEnter(BaseEventData _)
        {
            if (_isActive == true)
            {
                _inZone = true;
                StartCoroutine(Run());
            }
        }

        private void OnPointerExit(BaseEventData _)
        {
            _inZone = false;
        }

        private IEnumerator Run()
        {
            while (_inZone == true && _isActive == true)
            {
                _controllerMovement.Run(_multiplier);
                _restrictMovement.SetPosition();

                yield return null;
            }
        }
    }
}