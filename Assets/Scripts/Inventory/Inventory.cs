using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 3;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Item itemEmpty;
    private int currentIndexSlot = 0;
    private List<InventorySlot> slots = new List<InventorySlot>();

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
        foreach (var slot in slots)
        {
            if (slot.Item == itemEmpty)
            {
                slot.SetItem(newItem);
                return true;
            }
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
    }

    [ContextMenu("Next Slot")]
    public void NextSlot()
    {
        ModifyCurrentSlot(1);
    }

    [ContextMenu("Previous Slot")]
    public void PreviousSlot()
    {
        ModifyCurrentSlot(-1);
    }
}
