using UnityEngine;

public class Product : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] Sprite _projectImage;
    [SerializeField] Item _item;
    [SerializeField] int _price;
    [SerializeField] int _quantity;

    public int ID {  get { return _id; } }

    public Sprite ProjectImage { get { return _projectImage; } }

    public Item Item { get { return _item; } }

    public int Price { get { return _price; } }

    public int quantity { get { return _quantity; } }

    public Product(int id, Sprite projectImage, Item item, int price, int quantity)
    {
        _id = id;
        _projectImage = projectImage;
        _item = item;
        _price = price;
        _quantity = quantity;
    }
}
