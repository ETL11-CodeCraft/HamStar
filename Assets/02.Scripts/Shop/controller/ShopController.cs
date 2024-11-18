using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

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

    private int _columns = 2; // ��ǳ ��� ���̾ƿ�: ��

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

    public Product GetProduct(int id)
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

            obj.transform.SetParent(_scrollView.transform);
            //obj.transform.localPosition = new Vector3(-199 + (i % _columns) * 401,
            //    207 - (Mathf.Round(i / _columns) * 431), 0); // ��ǰ 2���� ��ġ
            //Debug.Log($"localPosition: [{i}] {-199 + (i % _columns) * 401}, {207 - Mathf.Round(i / _columns) * 431}");

            obj.transform.localPosition = new Vector3(202 + (i % _columns) * 401,
                -224 - (Mathf.Round(i / _columns) * 431), 0); // ��ǰ 2���� ��ġ
            Debug.Log($"localPosition: [{i}] {202 + (i % _columns) * 401}, {-224 - Mathf.Round(i / _columns) * 431}");
        }
    }

    private void Buy(Product product)
    {
        Debug.Log($"����=> coin: {GameManager.coin}, price: {product.price}, type: {product.type}");
        GameManager.coin -= product.price;
        RefeshCoin();
        if (product.type == ItemType.PlayGround)
        {
            product.quantity -= 1;
        }
        else
        {
            AddInventoryData(product.id);
        }
        SaveManager.inventoryData.coin = GameManager.coin;
        SaveManager.SaveInventoryData();
        CloseItemPopup();
    }

    private void Placement(Product product)
    {
        CloseItemPopup();
        CloseShop();

        // �¹��� ��ġ�ϱ�
        _placementController.HamsterWheelPrefab = product.prefab;
        _placementController.IsPlacementMode = true;
    }

    private void AddInventoryData(int productId)
    {
        var dict = SaveManager.inventoryData.quantityForProductId;
        if (dict.ContainsKey(productId))
        {
            SaveManager.inventoryData.quantityForProductId[productId] += 1;
        }
        else
        {
            SaveManager.inventoryData.quantityForProductId.Add(productId, 1);
        }
    }
}