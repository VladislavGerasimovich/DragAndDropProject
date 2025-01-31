using Environment;
using Items;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Scene
{
    public class DragAndDropHandler : TouchInputHandler
    {
        [SerializeField] private HoverButtons _hoverButtons;
        [SerializeField] private LayerMask _shelfLayerMask;
        [SerializeField] private LayerMask _defaultLayerMask;
        [SerializeField] private int _layerNumber;

        private bool _isMoving;
        private ItemPhysics _itemFall;
        private Coroutine _coroutine;

        public override void OnFingerDown(Finger finger)
        {
            if (MovementFinger == null)
            {
                bool isItem = CheckItems(finger.screenPosition);

                if (isItem == true)
                {
                    MovementFinger = finger;
                    _hoverButtons.SetActive(true);
                }
            }
        }

        public override void OnFingerMove(Finger finger)
        {
            if (finger == MovementFinger && _itemFall != null)
            {
                _isMoving = true;

                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }

                _coroutine = StartCoroutine(Run());
            }
        }

        public override void OnFingerUp(Finger finger)
        {
            if (finger == MovementFinger)
            {
                _isMoving = false;
                MovementFinger = null;
                RaycastHit2D hit = Raycast(finger.screenPosition, _shelfLayerMask);
                bool isShelf = CheckShelf(hit);

                if (isShelf == true)
                {
                    return;
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
                _itemFall = hit.transform.gameObject.GetComponent<ItemPhysics>();

                if (_itemFall != null)
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
            while (_isMoving == true)
            {
                Vector3 worldPosition = new Vector3(Camera.main.ScreenToWorldPoint(MovementFinger.screenPosition).x,
                    Camera.main.ScreenToWorldPoint(MovementFinger.screenPosition).y,
                    _itemFall.transform.position.z);
                _itemFall.transform.position = worldPosition;

                yield return null;
            }
        }

        private bool CheckShelf(RaycastHit2D hit)
        {
            if (hit.collider != null)
            {
                Shelf shelf = hit.transform.gameObject.GetComponent<Shelf>();

                if (shelf != null)
                {
                    ItemMove itemMove = _itemFall.GetComponent<ItemMove>();
                    itemMove.Run(shelf.transform.position);
                    _itemFall = null;
                    _hoverButtons.SetActive(false);

                    return true;
                }
            }

            return false;
        }
    }
}