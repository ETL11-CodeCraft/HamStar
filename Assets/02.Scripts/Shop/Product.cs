using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Product", menuName = "Scriptable Objects/Product", order = 99)]
public class Product : ScriptableObject
{
    public int id;
    public Sprite productImage;
    public string productName;
    [TextAreaAttribute]
    public string description;
    public int type;
    public int price;
    public int quantity;
    public GameObject prefab;
}
