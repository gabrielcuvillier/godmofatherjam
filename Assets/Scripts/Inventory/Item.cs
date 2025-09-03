using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private int count;
    [SerializeField] private int countMax;
    public int CountMax
    {
        get { return countMax; }
    }
    public int Count
    {
        get { return count; }
        set { count = value; }
    }

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
