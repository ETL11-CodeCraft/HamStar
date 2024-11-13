using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product Data", menuName = "Scriptable Objects/Product Data", order = 99)]
public class ProductData: ScriptableObject
{
    public List<Product> list;
}