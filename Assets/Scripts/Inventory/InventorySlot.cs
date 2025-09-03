using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI textCount;

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

        if (textCount != null && item != null)
        {
            if (item.CountMax > 1)
            {
                textCount.text = item.Count.ToString();
            }
            else
            {
                textCount.enabled = false;
            }
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
