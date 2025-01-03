﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    Canvas _shopCanvas;

    [SerializeField] GameObject _scrollView;
    [SerializeField] GameObject _itemPanel;
    [SerializeField] TextMeshProUGUI _coinText;

    [SerializeField] GameObject _slotPrefab;
    [SerializeField] ShopData _shopData;
    [SerializeField] ProductData _productData;

    [SerializeField] HamsterWheelPlacementController _placementController;

    private DataLoader _dataLoader;
    private InventoryData _inventoryData;
    private List<ProductSlot> _slots = new List<ProductSlot>(4);

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _inventoryData = _dataLoader.Load<InventoryData>();
    }

    public void OpenShop()
    {
        _shopCanvas.enabled = true;
        GameManager.instance.cantSwipe = true;
        RefreshShop();
        RefeshCoin();
    }

    public void CloseShop()
    {
        _shopCanvas.enabled = false;
        GameManager.instance.cantSwipe = false;
    }

    public void OpenItemPopup()
    {
        _itemPanel.SetActive(true);
    }

    public void CloseItemPopup()
    {
        _itemPanel.SetActive(false);
    }

    private Product GetProduct(int id)
    {
        return _productData.list.Find(v => v.id == id);
    }

    private void Start()
    {
        _shopCanvas = GetComponent<Canvas>();
        CloseShop();
        CloseItemPopup();
    }

    private void RefeshCoin()
    {
        _coinText.text = GameManager.instance.coin.ToString("N0") + "C";
        //GameManager.instance.UpdateCountUI(10001, GameManager.instance._seedCount);
        //GameManager.instance.UpdateCountUI(10002, GameManager.instance._goldseedCount);
        //GameManager.instance.UpdateCountUI(10003, GameManager.instance._potionCount);
    }

    private void RefreshShop()
    {
        _slots.ForEach(v => { Destroy(v.gameObject); });
        _slots.Clear();

        for (int i = 0; i < _shopData.productIds.Count; i++)
        {
            Product product = GetProduct(_shopData.productIds[i]);
            
            Debug.Log($"Product {product.id} {product.productName} {product.price}");

            GameObject obj = Instantiate(_slotPrefab);
            ProductSlot slot = obj.GetComponent<ProductSlot>();
            slot.ProjectImage = product.productImage;
            slot.Name = product.productName;
            slot.Price = product.price;
            slot.ClickEventHandler += () =>
            {
                SoundManager.instance.PlayButtonSound();
                OpenItemPopup();
                ProductController productController = _itemPanel.GetComponent<ProductController>();
                if (productController != null)
                {
                    productController.SetProduct(product);
                    productController.CancelAction = () =>
                    {
                        SoundManager.instance.PlayButtonSound();
                        CloseItemPopup();
                    };
                    productController.BuyAction = Buy;
                    productController.PlacementAction = Placement;
                    productController.Refresh();
                }
            };
            slot.RefreshSlot();

            // 이미 보유하고 있으면 구매 불가 처리
            InventoryItem found = _inventoryData.quantityForProductId.Find(v => v.productId == product.id);
            if (product.type == ItemType.PlayGround && found.productId != 0)
            {
                slot.SetSoldOut();
            }

            var rect = obj.GetComponent<RectTransform>();
            rect.SetParent(_scrollView.transform);
            rect.localScale = Vector3.one;

            _slots.Add(slot);
        }
    }

    private void Buy(Product product)
    {
        Debug.Log($"구매=> coin: {GameManager.instance.coin}, price: {product.price}, type: {product.type}");
        GameManager.instance.coin -= product.price;
        RefeshCoin();

        if (product.type == ItemType.Food)
        {
            AddInventoryData(product.id, 10);
        }
        else
        {
            AddInventoryData(product.id, 1);
        }

        _inventoryData.coin = GameManager.instance.coin;
        _dataLoader.Save(_inventoryData);

        // GameManager를 통해 UI 업데이트
        if (product.id == 10001) // 씨앗 ID
        {
            GameManager.instance.UpdateCountUI(10001, GameManager.instance._seedCount);
        }
        else if (product.id == 10002) // 골드 씨앗 ID
        {
            GameManager.instance.UpdateCountUI(10002, GameManager.instance._goldseedCount);
        }
        else if (product.id == 10003) // 물약 ID
        {
            GameManager.instance.UpdateCountUI(10003, GameManager.instance._potionCount);
        }

        CloseItemPopup();
    }

    private void Placement(Product product)
    {
        CloseItemPopup();
        CloseShop();
        _placementController.InfoVisible = true;
        // 쳇바퀴 배치하기
        _placementController.Product = product;
        _placementController.HamsterWheelPrefab = product.prefab;
        _placementController.IsPlacementMode = true;
        _placementController.CancelAction = () => {
            SoundManager.instance.PlayButtonSound();
            OpenShop();
            OpenItemPopup();
        };
        _placementController.ApplyAction = (product) =>
        {
            SoundManager.instance.PlayButtonSound();
            Buy(product);
        };
    }

    private void AddInventoryData(int productId, int added = 1)
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        var list = _inventoryData.quantityForProductId;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].productId == productId)
            {
                var quantity = list[i].quantity;
                _inventoryData.quantityForProductId.Remove(list[i]);

                InventoryItem availableItem = new InventoryItem();
                availableItem.productId = productId;
                availableItem.quantity = quantity + added;

                _inventoryData.quantityForProductId.Insert(i, availableItem);

                _dataLoader.Save(_inventoryData);

                return;
            }
        }

        InventoryItem item = new InventoryItem();
        item.productId = productId;
        item.quantity = added;
        _inventoryData.quantityForProductId.Add(item);

        _dataLoader.Save(_inventoryData);
    }
}
