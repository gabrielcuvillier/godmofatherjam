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

    [ContextMenu("Next Slot")]
    private void NextSlot()
    {
        ModifyCurrentSlot(1);
    }

    [ContextMenu("Previous Slot")]
    private void PreviousSlot()
    {
        ModifyCurrentSlot(-1);
    }
}
