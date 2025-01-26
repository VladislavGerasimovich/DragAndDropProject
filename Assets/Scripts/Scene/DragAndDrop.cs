using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DragAndDrop : TouchInput, ICheckable
{
    [SerializeField] private HoverButtons _hoverButtons;
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
                _hoverButtons.SetActive(true);
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
                    ItemMove itemMove = _itemFall.GetComponent<ItemMove>();
                    itemMove.Run(shelf.transform.position);
                    _itemFall = null;
                    _hoverButtons.SetActive(false);

                    return;
                }
            }

            _itemFall.Resolve();
            _itemFall = null;
            _hoverButtons.SetActive(false);
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
                ItemMove itemMove = _itemFall.GetComponent<ItemMove>();
                itemMove.Stop();
                _itemFall.Reject();
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