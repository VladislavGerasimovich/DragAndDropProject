using System.Collections;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    private float _speed;
    private bool _isMoving;

    public Coroutine Coroutine;

    private void Awake()
    {
        _speed = 2f;
    }

    public void Run(Vector3 position)
    {
        _isMoving = true;
        Coroutine = StartCoroutine(RunCoroutine(position));
    }

    public void Stop()
    {
        _isMoving = false;
    }

    private IEnumerator RunCoroutine(Vector3 position)
    {
        Vector3 newPositon = new Vector3(position.x, position.y, transform.position.z);

        while (transform.position != position && _isMoving == true)
        {
            transform.position = Vector3.Lerp(transform.position, newPositon, _speed * Time.deltaTime);

            yield return null;
        }

        Coroutine = null;
    }
}