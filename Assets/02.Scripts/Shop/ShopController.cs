using System.Collections;
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

    private int _columns = 2; // 상풍 목록 레이아웃: 열

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
        for (int i = 0; i < _shopData.list.Count; i++)
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
                    productController.Refresh();
                }
            };
            slot.RefreshSlot();

            obj.transform.SetParent(_scrollView.transform);
            //obj.transform.localPosition = new Vector3(-199 + (i % _columns) * 401,
            //    207 - (Mathf.Round(i / _columns) * 431), 0); // 상품 2열로 배치
            //Debug.Log($"localPosition: [{i}] {-199 + (i % _columns) * 401}, {207 - Mathf.Round(i / _columns) * 431}");

            obj.transform.localPosition = new Vector3(202 + (i % _columns) * 401,
                -224 - (Mathf.Round(i / _columns) * 431), 0); // 상품 2열로 배치
            Debug.Log($"localPosition: [{i}] {202 + (i % _columns) * 401}, {-224 - Mathf.Round(i / _columns) * 431}");
        }
    }

    private void Buy(Product product)
    {
        Debug.Log($"구매=> coin: {GameManager.coin}, price: {product.price}");
        GameManager.coin -= product.price;
        product.quantity -= 1;
        RefeshCoin();
        _itemPanel.SetActive(false);
    }
}
