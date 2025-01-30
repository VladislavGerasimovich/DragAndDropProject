using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;

public class HoverButton : MonoBehaviour
{
    [SerializeField] private ControllerPosition _controllerMovement;
    [SerializeField] private float _multiplier;

    private bool _isActive;
    private Button _button;
    private bool _inZone;

    private delegate void OnPointerDelegate();

    private void Start()
    {
        _button = GetComponent<Button>();
        EventTrigger trigger = _button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        OnPointerDelegate onPointerDelegate = new OnPointerDelegate(OnPointerEnter);
        CreateEvent(trigger, entry, EventTriggerType.PointerEnter, onPointerDelegate);
        entry = new EventTrigger.Entry();
        onPointerDelegate = new OnPointerDelegate(OnPointerExit);
        CreateEvent(trigger, entry, EventTriggerType.PointerExit, onPointerDelegate);
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    private void CreateEvent(EventTrigger trigger,
        Entry entry,
        EventTriggerType type,
        OnPointerDelegate onPointerDelegate)
    {
        entry.eventID = type;
        entry.callback.AddListener((data) => { onPointerDelegate(); });
        trigger.triggers.Add(entry);
    }

    private void OnPointerEnter()
    {
        if (_isActive == true)
        {
            _inZone = true;
            StartCoroutine(Run());
        }
    }

    private void OnPointerExit()
    {
        _inZone = false;
    }

    private IEnumerator Run()
    {
        while (_inZone == true && _isActive == true)
        {
            _controllerMovement.Run(_multiplier);

            yield return null;
        }
    }
}