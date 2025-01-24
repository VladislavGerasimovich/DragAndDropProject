using UnityEngine;

public class ControllerMovement : MonoBehaviour
{
    private float _speed;

    private void Awake()
    {
        _speed = 0.1f;
    }

    public void Run(float newPositionX)
    {
        transform.position = new Vector2(transform.position.x + newPositionX * _speed, transform.position.y);
    }
}