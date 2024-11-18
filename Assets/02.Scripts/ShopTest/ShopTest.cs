using TMPro;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _foodText;
    [SerializeField] TextMeshProUGUI _medicineText;
    [SerializeField] private ProductData _productData;
    private DataLoader _dataLoader;
    private InventoryData _inventoryData;
    private PlacementData _placementData;

    private void Awake()
    {
        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        RefreshText();

        LoadPlacementData();
    }

    public void RefreshText()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        GameManager.coin = _inventoryData.coin;
        _coinText.text = $"코인 ({GameManager.coin})";
        _foodText.text = $"일반 먹이 ({GetQuantityForId(0)})\n특수 먹이 ({GetQuantityForId(1)})";
        _medicineText.text = $"치료 물약 ({GetQuantityForId(2)})";
    }

    private int GetQuantityForId(int id)
    {
        var item = _inventoryData.quantityForProductId.Find((x) => x.productId == id);
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

    private void LoadPlacementData()
    {
        // 쳇바퀴 배치 정보 로드
        _placementData = _dataLoader.Load<PlacementData>();

        _placementData.placements.ForEach(p =>
        {
            Product product = GetProduct(p.productId);
            Instantiate(product.prefab, p.position, p.rotation);
        });
    }

    private Product GetProduct(int id)
    {
        return _productData.list.Find((v) => v.id == id);
    }
}
