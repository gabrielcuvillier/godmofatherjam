using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputsActions _actions;

    [Header("References")]
    [SerializeField] InputActionAsset _inputs;
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody _rigidbody;

    [Header("Speeds")]
    [SerializeField] float _playerSpeed = 30f;
    [SerializeField] Vector2 _cameraRotationSpeed = new Vector2(30f,30f);

    private void OnEnable()
    {
        _actions = new PlayerInputsActions();
        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        _actions.Player.Disable();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }

    void UpdateRotation()
    {
        Vector2 mouseDirection = _actions.Player.Look.ReadValue<Vector2>();
        mouseDirection.Normalize();
        Vector3 cameraVerticalRotation = new Vector3(-mouseDirection.y, 0, 0);
        Vector3 cameraHorizontalRotation = new Vector3(0, mouseDirection.x, 0);
        transform.localEulerAngles += cameraHorizontalRotation * _cameraRotationSpeed.x * Time.deltaTime;
        _camera.transform.localEulerAngles += cameraVerticalRotation * _cameraRotationSpeed.y * Time.deltaTime;
    }

    void UpdatePosition()
    {
        Vector2 inputDirection = _actions.Player.Move.ReadValue<Vector2>();
        inputDirection.Normalize();
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 direction = forward * inputDirection.y + right * inputDirection.x;
        
        _rigidbody.linearVelocity = direction * _playerSpeed + new Vector3(0f, _rigidbody.linearVelocity.y, 0f);
    }
}
