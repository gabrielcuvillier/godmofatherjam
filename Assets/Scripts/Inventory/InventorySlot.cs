using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private Item item;
    public Item Item
    {
        get { return item; }
        set { item = value; }
    }

    [SerializeField] private Image image;
    [SerializeField] private Image imageContour;

    public void SetItem(Item newItem)
    {
        Item = newItem;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (image != null && item != null)
        {
            image.sprite = item.Sprite;
        }
    }

    public void SetIsTarget(bool isTarget)
    {
        if (imageContour != null)
        {
            imageContour.enabled = isTarget;
        }
    }
}
