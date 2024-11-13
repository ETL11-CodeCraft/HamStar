using TMPro;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _foodText;
    [SerializeField] TextMeshProUGUI _medicineText;

    private void Start()
    {
        SaveManager.LoadInventoryData();
        RefreshText();
    }

    public void RefreshText()
    {
        GameManager.coin = SaveManager.inventoryData.coin;
        _coinText.text = $"���� ({GameManager.coin})";
        _foodText.text = $"�Ϲ� ���� ({GetQuantityForId(0)})\nƯ�� ���� ({GetQuantityForId(1)})";
        _medicineText.text = $"ġ�� ���� ({GetQuantityForId(2)})";
    }

    private int GetQuantityForId(int id)
    {
        SaveManager.inventoryData.quantityForProductId.TryGetValue(id, out int quantity);
        return quantity;
    }

    public void AddCoin()
    {
        GameManager.coin += 10;
        SaveManager.inventoryData.coin = GameManager.coin;
        SaveManager.SaveInventoryData();
        RefreshText();
    }

    public void Save()
    {
        SaveManager.SaveInventoryData();
    }

}
