using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemFall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Floor floor))
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
}