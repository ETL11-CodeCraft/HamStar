using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        RefreshShop();
        RefeshCoin();
    }

    public void CloseShop()
    {
        _shopCanvas.enabled = false;
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
        _coinText.text = GameManager.coin + "C";
    }

    private void RefreshShop()
    {
        _slots.ForEach(v => { Destroy(v.gameObject); });
        _slots.Clear();

        for (int i = 0; i < _shopData.productIds.Count; i++)
        {
            Product product = GetProduct(i);
            
            Debug.Log($"Product {product.id} {product.productName} {product.price} {product.quantity}");

            GameObject obj = Instantiate(_slotPrefab);
            ProductSlot slot = obj.GetComponent<ProductSlot>();
            slot.ProjectImage = product.productImage;
            slot.Name = product.productName;
            slot.Price = product.price;
            slot.ClickEventHandler += () =>
            {
                OpenItemPopup();
                ProductController productController = _itemPanel.GetComponent<ProductController>();
                if (productController != null)
                {
                    productController.SetProduct(product);
                    productController.BuyAction = Buy;
                    productController.PlacementAction = Placement;
                    productController.Refresh();
                }
            };
            slot.RefreshSlot();

            if (product.quantity <= 0) // 상품 재고가 없으면 구매 불가
            {
                slot.enabled = false;
            }

            var rect = obj.GetComponent<RectTransform>();
            rect.SetParent(_scrollView.transform);
            rect.localScale = Vector3.one;

            _slots.Add(slot);
        }
    }

    private void Buy(Product product)
    {
        Debug.Log($"구매=> coin: {GameManager.coin}, price: {product.price}, type: {product.type}");
        GameManager.coin -= product.price;
        RefeshCoin();

        if (product.type == ItemType.PlayGround)
        {
            product.quantity -= 1;
        }
        else if (product.type == ItemType.Food)
        {
            AddInventoryData(product.id, 10);
        }
        else if (product.type == ItemType.Medicine)
        {
            AddInventoryData(product.id, 1);
        }

        _inventoryData.coin = GameManager.coin;
        _dataLoader.Save(_inventoryData);

        CloseItemPopup();
    }

    private void Placement(Product product)
    {
        CloseItemPopup();
        CloseShop();

        // 쳇바퀴 배치하기
        _placementController.Product = product;
        _placementController.HamsterWheelPrefab = product.prefab;
        _placementController.IsPlacementMode = true;
        _placementController.CancelAction = () => { 
            OpenShop();
            OpenItemPopup();
        };
        _placementController.ApplyAction = Buy;
    }

    private void AddInventoryData(int productId, int added = 1)
    {
        var list = _inventoryData.quantityForProductId;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].productId == productId)
            {
                var quantity = list[i].quantity;
                _inventoryData.quantityForProductId.Remove(list[i]);

                inventoryItem availableItem = new inventoryItem();
                availableItem.productId = productId;
                availableItem.quantity = quantity + added;

                _inventoryData.quantityForProductId.Insert(i, availableItem);

                _dataLoader.Save(_inventoryData);

                return;
            }
        }

        inventoryItem item = new inventoryItem();
        item.productId = productId;
        item.quantity = added;
        _inventoryData.quantityForProductId.Add(item);

        _dataLoader.Save(_inventoryData);
    }
}
