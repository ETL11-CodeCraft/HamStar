using TMPro;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _foodText;
    [SerializeField] TextMeshProUGUI _medicineText;
    private DataLoader _dataLoader;
    private InventoryData _inventoryData;

    private void Awake()
    {
        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        RefreshText();
    }

    public void RefreshText()
    {
        GameManager.coin = _inventoryData.coin;
        _coinText.text = $"���� ({GameManager.coin})";
        _foodText.text = $"�Ϲ� ���� ({GetQuantityForId(0)})\nƯ�� ���� ({GetQuantityForId(1)})";
        _medicineText.text = $"ġ�� ���� ({GetQuantityForId(2)})";
    }

    private int GetQuantityForId(int id)
    {
        var item = _inventoryData.quantityForProductId.Find((x) => { if (x.productId == id) return true; return false; });
        return item.quantity;
    }

    public void AddCoin()
    {
        GameManager.coin += 10;
        _inventoryData.coin = GameManager.coin;
        _dataLoader.Save(_inventoryData);
        RefreshText();
    }

    public void Save()
    {
        _dataLoader.Save(_inventoryData);
    }

}
