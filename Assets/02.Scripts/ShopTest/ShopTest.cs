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
        _coinText.text = $"코인 ({GameManager.coin})";
        _foodText.text = $"일반 먹이 ({GetQuantityForId(0)})\n특수 먹이 ({GetQuantityForId(1)})";
        _medicineText.text = $"치료 물약 ({GetQuantityForId(2)})";
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
