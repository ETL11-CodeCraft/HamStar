using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _productNameText;
    [SerializeField] TextMeshProUGUI _productDescriptionText;
    [SerializeField] Image _productImage;
    [SerializeField] Button _cancelButton;
    [SerializeField] Button _buyButton;

    [SerializeField] GameObject _shopSuccessPanel;
    [SerializeField] GameObject _shopFailPanel;

    Product _product;

    public Action<Product> BuyAction;
    public Action<Product> PlacementAction;

    public void SetProduct(Product product)
    {
        _product = product;
    }

    public void Refresh()
    {
        _productNameText.text = _product.productName;
        _productDescriptionText.text = _product.description;
        _productImage.sprite = _product.productImage;
        
        _buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _product.price + "C";

        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(() =>
        {
            if (GameManager.coin < _product.price)
            {
                PopupUI ui = _shopFailPanel.GetComponent<PopupUI>();
                ui.Popup();
            }
            else if (_product.type != ItemType.PlayGround)
            {
                BuyAction?.Invoke(_product);
                PopupUI ui = _shopSuccessPanel.GetComponent<PopupUI>();
                ui.Popup();
            }
            else if (_product.type == ItemType.PlayGround)
            {
                // 설치 모드로
                PlacementAction.Invoke(_product);
            }
        });
    }

    private void Start()
    {
        _shopSuccessPanel.SetActive(false);
        _shopFailPanel.SetActive(false);
    }
}
