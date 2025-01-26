using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MovementZone : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _multiplier;
    [SerializeField] private ControllerMovement _controllerMovement;

    private bool _inZone;
    private Coroutine _coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ItemFall itemfall))
        {
            _inZone = true;
            _coroutine = StartCoroutine(Run());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ItemFall itemfall))
        {
            StopCoroutine(_coroutine);
            _inZone = false;
            _coroutine = null;
        }
    }

    private IEnumerator Run()
    {
        while(_inZone == true)
        {
            _controllerMovement.Run(_multiplier);

            yield return null;
        }
    }
}