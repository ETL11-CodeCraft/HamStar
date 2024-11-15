using UnityEngine;


public class Item : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Installation
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public ItemType Type { get; private set; }

    public Vector3 Size { get; private set; }

    public Item(string name, string description, ItemType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }

    public Item(string name, string description, ItemType type, Vector3 size)
    {
        Name = name;
        Description = description;
        Type = type;
        Size = size;
    }
}
