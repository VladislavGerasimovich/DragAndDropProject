using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DragAndDrop : TouchInput, ICheckable
{
    [SerializeField] private List<ButtonHover> _buttonsHover;
    [SerializeField] private int _layerNumber;

    private int _shelfLayerMask;
    private int _defaultLayerMask;
    private bool _isMoving;
    private ItemFall _itemFall;
    private Coroutine _coroutine;

    private void Awake()
    {
        _shelfLayerMask = 1 << _layerNumber;
        _defaultLayerMask = 1 << 0;
    }

    public override void OnFingerDown(Finger finger)
    {
        if (_movementFinger == null)
        {
            bool isItem = CheckItems(finger.screenPosition);

            if (isItem == true)
            {
                _movementFinger = finger;

                foreach (ButtonHover button in _buttonsHover)
                {
                    button.SetActive(true);
                }
            }
        }
    }

    public override void OnFingerMove(Finger finger)
    {
        if (finger == _movementFinger && _itemFall != null)
        {
            _isMoving = true;

            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(Run());
        }
    }

    public override void OnFingerUp(Finger finger)
    {
        if (finger == _movementFinger)
        {
            _isMoving = false;
            _movementFinger = null;
            
            RaycastHit2D hit = Raycast(finger.screenPosition, _shelfLayerMask);

            if (hit.collider != null)
            {
                Shelf shelf = hit.transform.gameObject.GetComponent<Shelf>();

                if(shelf != null)
                {
                    _itemFall = null;

                    return;
                }
            }
            

            _itemFall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _itemFall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            _itemFall = null;

            foreach (ButtonHover button in _buttonsHover)
            {
                button.SetActive(false);
            }

        }
    }

    public bool CheckItems(Vector2 position)
    {
        RaycastHit2D hit = Raycast(position, _defaultLayerMask);

        if (hit.collider != null)
        {
            _itemFall = hit.transform.gameObject.GetComponent<ItemFall>();

            if(_itemFall != null )
            {
                _itemFall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                _itemFall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }

        if (_itemFall != null)
        {
            return true;
        }

        return false;
    }

    private RaycastHit2D Raycast(Vector2 position, int layerMask)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, layerMask);

        return hit;
    }

    private IEnumerator Run()
    {
        while(_isMoving == true)
        {
            Vector3 worldPosition = new Vector3(Camera.main.ScreenToWorldPoint(_movementFinger.screenPosition).x,
                Camera.main.ScreenToWorldPoint(_movementFinger.screenPosition).y,
                _itemFall.transform.position.z);
            _itemFall.transform.position = worldPosition;

            yield return null;
        }
    }
}