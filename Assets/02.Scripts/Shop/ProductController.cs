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
    
    Product _product;

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
    }
}
