using System.Collections;
using UnityEngine;

namespace Items 
{
    public class ItemMove : MonoBehaviour
    {
        private float _speed;
        private Coroutine _coroutine;

        private void Awake()
        {
            _speed = 2f;
        }

        public void Run(Vector3 position)
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(RunCoroutine(position));
            }
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator RunCoroutine(Vector3 position)
        {
            Vector3 newPositon = new Vector3(position.x, position.y, transform.position.z);

            while (transform.position != position)
            {
                transform.position = Vector3.Lerp(transform.position, newPositon, _speed * Time.deltaTime);

                yield return null;
            }

            _coroutine = null;
        }
    }
}