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

    private void OnTriggerExit2D(Collider2D collision)
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }


    public void Resolve()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Reject()
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}