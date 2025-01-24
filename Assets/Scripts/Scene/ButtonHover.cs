using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] private ControllerMovement _controllerMovement;
    [SerializeField] private float _multiplier;

    private bool _isActive;
    private Button button;
    private bool _inZone;
    private Coroutine _coroutine;

    private void Start()
    {
        button = GetComponent<Button>();
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(entry);
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    private void OnPointerEnter()
    {
        if (_isActive == true)
        {
            _inZone = true;
            _coroutine = StartCoroutine(Run());
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