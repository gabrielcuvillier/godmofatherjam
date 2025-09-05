using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] UnityEvent OnAttackEvent;

    [SerializeField] Inventory _inventory;
    [SerializeField] PlayerInputs _playerInputs;

    int _currentSlotIndex = 0;
    WeaponUsage _currentWeapon = null;
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
            _currentWeapon = null;
            OnAttackEvent.Invoke();
            _inventory.PassToNextSlot();
        }
    }

    private void OnItemSelected(Item item, int slotIndex)
    {
        _currentWeapon?.gameObject.SetActive(false);
        _currentSlotIndex = slotIndex;
        if (item != _inventory.EmptyItem)
        {
            WeaponUsage newWeapon = _inventory.GetWeaponForItem(slotIndex);
            newWeapon.gameObject.SetActive(true);
            newWeapon.Select();
            _currentWeapon = newWeapon;
        }
        else
        {
            _currentWeapon = null;
        }
    }

    private void OnDestroy()
    {
        if (_inventory != null)
            _inventory.OnItemSelected -= OnItemSelected;
        if (_playerInputs != null)
            _playerInputs.OnAttack -= OnAttack;
    }
}
