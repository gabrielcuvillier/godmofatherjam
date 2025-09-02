using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string itemName;
    [SerializeField] private string description;

    public Sprite Sprite
    {
        get { return sprite; }
    }

    public string ItemName
    {
        get { return itemName; }
    }

    public string Description
    {
        get { return description; }
    }

}
