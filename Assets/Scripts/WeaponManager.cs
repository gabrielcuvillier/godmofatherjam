using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UnityEvent OnAttackEvent;

    [SerializeField] Inventory _inventory;
    [SerializeField] PlayerInputs _playerInputs;

    [Header("Link item/weapon")]
    [SerializeField] List<Item> _itemEntries;
    [SerializeField] List<WeaponUsage> _weaponEntries;

    int _currentSlotIndex = 0;
    WeaponUsage _currentWeapon;
    public WeaponUsage CurrentWeapon => _currentWeapon;

    private void Start()
    {
        if (_inventory != null)
            _inventory.OnItemSelected += OnItemSelected;
        if (_playerInputs != null)
            _playerInputs.OnAttack += OnAttack;
    }

    private void OnAttack()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.Use();
            _inventory.RemoveItem(_currentSlotIndex);
            OnAttackEvent.Invoke();
        }
    }

    private void OnItemSelected(Item item, int slotIndex)
    {
        _currentWeapon?.gameObject.SetActive(false);
        _currentSlotIndex = slotIndex;
        if (item != _inventory.EmptyItem)
        {
            int indexWeapon = _itemEntries.IndexOf(item);
            if (_weaponEntries != null && _weaponEntries.Count > indexWeapon)
            {
                WeaponUsage newWeapon = _weaponEntries[indexWeapon];
                newWeapon.gameObject.SetActive(true);
                newWeapon.Select();
                _currentWeapon = newWeapon;
            }
            else
            {
                Debug.LogWarning("Weapon entries is not initialized as wanted.");
            }
        }
    }

    private void OnDestroy()
    {
        _inventory.OnItemSelected -= OnItemSelected;
        _playerInputs.OnAttack -= OnAttack;
    }
}
