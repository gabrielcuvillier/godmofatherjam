using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public event Action<Item, int> OnItemSelected;
    [SerializeField] UnityEvent OnItemSelectedEvent;
    [SerializeField] UnityEvent OnItemRemovedEvent;

    [SerializeField] private int maxSlots = 3;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Item itemEmpty;
    private int currentIndexSlot = 0;
    private List<InventorySlot> slots = new List<InventorySlot>();
    public Item EmptyItem => itemEmpty;

    [Header("Link item/weapon")]
    [SerializeField] List<Item> _itemEntries;
    [SerializeField] List<WeaponUsage> _weaponEntries;
    WeaponUsage[] _slotsWeapons;
    [SerializeField] GameObject _weaponParent;
    [SerializeField] GameObject _ShotPoint;

    void Start()
    {
        // Initialiser les slots d'inventaire au démarage
        if (itemEmpty == null)
        {
            Debug.LogError("Item vide non assigné !");
            return;
        }
        if (slotPrefab == null)
        {
            Debug.LogError("Prefab de slot non assigné !");
            return;
        }
        for (int i = 0; i < maxSlots; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, transform);
            newSlot.SetItem(itemEmpty);
            slots.Add(newSlot);
        }

        slots[currentIndexSlot].SetIsTarget(true);
        _slotsWeapons = new WeaponUsage[maxSlots];


    }

    private void ModifyCurrentSlot(int amount)
    {
        slots[currentIndexSlot].SetIsTarget(false);
        currentIndexSlot += amount;
        if (currentIndexSlot < 0)
        {
            currentIndexSlot = slots.Count - 1;
        }
        else if (currentIndexSlot >= slots.Count)
        {
            currentIndexSlot = currentIndexSlot - maxSlots;
        }

        slots[currentIndexSlot].SetIsTarget(true);

        OnItemSelected?.Invoke(slots[currentIndexSlot].Item, currentIndexSlot);
        OnItemSelectedEvent.Invoke();
    }

    private bool IsInventoryFull()
    {
        foreach (var slot in slots)
        {
            if (slot.Item == itemEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public bool AddItem(Item newItem)
    {
        if (IsInventoryFull())
        {
            Debug.LogWarning("Inventaire plein !");
            return false;
        }

        // Ajouter l'élément à l'emplacement vide
        int slotIndex = 0;
        foreach (var slot in slots)
        {
            if (slot.Item == itemEmpty)
            {
                slot.SetItem(newItem);

                //int indexNewItem = _itemEntries.FindIndex(item => item.name == newItem.name);
                int indexNewItem = -1;
                for (int i = 0; i < _itemEntries.Count && indexNewItem == -1; i++)
                {
                    if (_itemEntries[i].name == newItem.name)
                    {
                        indexNewItem = i;
                    }
                }
                if (_weaponEntries != null && _weaponEntries.Count > indexNewItem)
                {
                    WeaponUsage weapon = Instantiate(_weaponEntries[indexNewItem], _weaponParent?.transform);
                    weapon.Initialize(_ShotPoint);
                    _slotsWeapons[slotIndex] = weapon;
                    if (currentIndexSlot != slotIndex)
                    {
                        weapon.gameObject.SetActive(false);
                    }
                }

                OnItemSelected?.Invoke(slots[currentIndexSlot].Item, currentIndexSlot);
                OnItemSelectedEvent.Invoke();
                return true;
            }
            slotIndex++;
        }

        return false;
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= slots.Count)
        {
            Debug.LogError("Index de slot invalide !");
            return;
        }

        slots[currentIndexSlot].SetIsTarget(false);
        currentIndexSlot = index;
        slots[currentIndexSlot].SetIsTarget(true);

        OnItemSelected?.Invoke(slots[currentIndexSlot].Item, currentIndexSlot);
        OnItemSelectedEvent.Invoke();
    }

    [ContextMenu("Next Slot")]
    public void NextSlot()
    {
        Debug.Log("NextSlot");
        ModifyCurrentSlot(-1);
    }

    [ContextMenu("Previous Slot")]
    public void PreviousSlot()
    {
        Debug.Log("PreviousSlot");
        ModifyCurrentSlot(1);
    }

    public void RemoveItem(int index)
    {
        slots[index].SetItem(itemEmpty);
    }

    public void PassToNextSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item != itemEmpty)
            {
                SelectItem(i);
                return;
            }
        }
    }

    public WeaponUsage GetWeaponForItem(int slotIndex)
    {
        int indexWeapon = slotIndex; //_itemEntries.IndexOf(item);
        if (_slotsWeapons.Length > indexWeapon)
        {
            WeaponUsage newWeapon = _slotsWeapons[indexWeapon];
            return newWeapon;
        }
        else
        {
            Debug.LogWarning("Weapon entries is not initialized as wanted.");
            return null;
        }
    }
}
