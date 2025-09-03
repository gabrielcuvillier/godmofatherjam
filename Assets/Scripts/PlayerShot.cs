using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerShot : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shotPoint;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();
        playerInput.actions["Attack"].performed += ctx => Shot();
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
    }

    private void Shot()
    {
        Debug.Log("PlayerShoot");
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        bullet.GetComponent<BulletController>().Initialize(shotPoint.forward);
    }
}
