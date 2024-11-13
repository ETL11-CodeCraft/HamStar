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
        _coinText.text = $"코인 ({GameManager.coin})";
        _foodText.text = $"일반 먹이 ({GetQuantityForId(0)})\n특수 먹이 ({GetQuantityForId(1)})";
        _medicineText.text = $"치료 물약 ({GetQuantityForId(2)})";
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
