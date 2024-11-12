using UnityEngine;

public class ShopController : MonoBehaviour
{
    Canvas _shopCanvas;

    [SerializeField] GameObject _scrollView;
    [SerializeField] GameObject _itemPanel;
    [SerializeField] GameObject _shopSuccessPanel;
    [SerializeField] GameObject _shopFailPanel;

    [SerializeField] GameObject _slotPrefab;
    [SerializeField] ShopData _shopData;
    [SerializeField] ProductData _productData;

    private int _columns = 2;

    public void OpenShop()
    {
        _shopCanvas.enabled = true;
        RefreshShop();
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
        _shopSuccessPanel.SetActive(false);
        _shopFailPanel.SetActive(false);
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
}
