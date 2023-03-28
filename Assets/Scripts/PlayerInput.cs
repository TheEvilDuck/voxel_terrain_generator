using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]OnMoveEvent _onMoveEvent;
    [SerializeField]OnMouseMovement _onCameraMoveEvent;
    [SerializeField]UnityEvent _leftMouseClicked;
    [SerializeField]UnityEvent _rightMouseClicked;
    [SerializeField]float _mouseSensetivity = 4f;
    [SerializeField]float _minCameraAngle =0;
    [SerializeField]float _maxCameraAngle = 90f;
    private Vector2 _moveVector;
    private Vector2 _cameraAngle;
    private float _rotationX;

    private void MouseAiming ()
    {
        _cameraAngle.x += Input.GetAxis("Mouse X") * _mouseSensetivity;
		_cameraAngle.y += Input.GetAxis("Mouse Y") * _mouseSensetivity;
		_cameraAngle.y = Mathf.Clamp(_cameraAngle.y, -_minCameraAngle, _maxCameraAngle);
		var xQuat = Quaternion.AngleAxis(_cameraAngle.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(_cameraAngle.y, Vector3.left);
        _onCameraMoveEvent?.Invoke(xQuat,yQuat);
    }
    private void KeyBoardMovement()
    {
        if (_moveVector.x!=Input.GetAxis("Horizontal"))
        {
            _moveVector.x = Input.GetAxis("Horizontal");
            _onMoveEvent?.Invoke(_moveVector);
        }
        if (_moveVector.y!=Input.GetAxis("Vertical"))
        {
            _moveVector.y = Input.GetAxis("Vertical");
            _onMoveEvent?.Invoke(_moveVector);
        }
    }
    void Update()
    {
        KeyBoardMovement();
        MouseAiming();
        if (Input.GetMouseButtonDown(0))
        {
            _leftMouseClicked?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
             _rightMouseClicked?.Invoke();
        }
    }
    [System.Serializable]
    class OnMoveEvent: UnityEvent<Vector2>{}
    [System.Serializable]
    class OnMouseMovement: UnityEvent<Quaternion,Quaternion>{}
}
