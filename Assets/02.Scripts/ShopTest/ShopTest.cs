﻿using TMPro;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _seedCountText;
    [SerializeField] TextMeshProUGUI _goldSeedCountText;
    [SerializeField] TextMeshProUGUI _medicineText;
    [SerializeField] ProductData _productData;

    private DataLoader _dataLoader;
    private InventoryData _inventoryData;
    private PlacementData _placementData;

    private HamsterWheel _wheel;

    private void Awake()
    {
        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        RefreshInventoryData();
        RefreshPlacementData();
    }

    public void RefreshInventoryData()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        GameManager.instance.coin = _inventoryData.coin;
        _seedCountText.text = $"{GetQuantityForId(0)}";
        _goldSeedCountText.text = $"{GetQuantityForId(1)}";
        _medicineText.text = $"{GetQuantityForId(2)}";
    }

    private int GetQuantityForId(int id)
    {
        var item = _inventoryData.quantityForProductId.Find((x) => x.productId == id);
        return item.quantity;
    }

    public void AddCoin()
    {
        GameManager.instance.coin += 10;
        _inventoryData.coin = GameManager.instance.coin;
        _dataLoader.Save(_inventoryData);
        RefreshInventoryData();
    }

    private void RefreshPlacementData()
    {
        // 쳇바퀴 배치 정보 로드
        _placementData = _dataLoader.Load<PlacementData>();

        _placementData.placements.ForEach(p =>
        {
            Product product = GetProduct(p.productId);
            _wheel = Instantiate(product.prefab, p.position, p.rotation).GetComponent<HamsterWheel>();
        });
    }

    private Product GetProduct(int id)
    {
        return _productData.list.Find((v) => v.id == id);
    }
}
