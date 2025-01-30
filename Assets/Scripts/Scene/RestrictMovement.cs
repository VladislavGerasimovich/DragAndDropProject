using Cinemachine;
using UnityEngine;

public class RestrictMovement : MonoBehaviour
{
    [SerializeField] private Transform _cameraControllerPosition;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Camera _camera;

    public void SetPosition()
    {
        if (Mathf.Abs(_cinemachineVirtualCamera.transform.position.x) > Mathf.Abs(_camera.transform.position.x))
        {
            _cameraControllerPosition.position = new Vector2(_camera.transform.position.x, _cinemachineVirtualCamera.transform.position.y);
        }
    }
}