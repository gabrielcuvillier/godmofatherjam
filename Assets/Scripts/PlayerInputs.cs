using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputs : MonoBehaviour
{
    PlayerInputsActions _actions;

    [Header("References")]
    [SerializeField] InputActionAsset _inputs;
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody _rigidbody;

    [Header("Speeds")]
    [SerializeField] float _playerSpeed = 30f;
    [SerializeField] Vector2 _cameraRotationSpeed = new Vector2(30f, 30f);
    public event Action OnAttack;

    [SerializeField] float maxRotation = 80f;

    private void OnEnable()
    {
        _actions = new PlayerInputsActions();
        _actions.Player.Enable();
        _actions.Player.Attack.started += OnAttackInput;
    }

    private void OnAttackInput(InputAction.CallbackContext obj)
    {
        OnAttack?.Invoke();
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnAttackInput;
        _actions.Player.Disable();
    }

    void Start()
    {
        GameManager.Instance.DisableCursor();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        Vector2 mouseDirection = _actions.Player.Look.ReadValue<Vector2>();
        mouseDirection.Normalize();
        Vector3 cameraVerticalRotation = new Vector3(-mouseDirection.y, 0, 0);
        Vector3 cameraHorizontalRotation = new Vector3(0, mouseDirection.x, 0);
        transform.localEulerAngles += cameraHorizontalRotation * _cameraRotationSpeed.x * Time.deltaTime;
        Vector3 rotationCamera = cameraVerticalRotation * _cameraRotationSpeed.y * Time.deltaTime;
        float cameraRotation = _camera.transform.localEulerAngles.x + rotationCamera.x;
        if (cameraRotation >  maxRotation && cameraRotation < 180)
        {
            cameraRotation = maxRotation;
        } else if (cameraRotation < (360 - maxRotation) && cameraRotation >= 180)
        {
            cameraRotation = 360 - maxRotation;
        }
        rotationCamera.x = cameraRotation;
        _camera.transform.localEulerAngles = rotationCamera;
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
