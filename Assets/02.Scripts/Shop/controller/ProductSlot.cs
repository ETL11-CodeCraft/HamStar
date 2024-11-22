using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductSlot : MonoBehaviour
{
    [SerializeField] Sprite _productImage;
    [SerializeField] string _name;
    [SerializeField] int _price;

    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _priceText;

    private Button _button;

    public Sprite ProjectImage { get { return _productImage; } set { _productImage = value; } }

    public string Name { get { return _name; } set { _name = value; } }

    public int Price { get { return _price; } set { _price = value; } }

    public event UnityAction ClickEventHandler;

    public void RefreshSlot()
    {
        _image.sprite = _productImage;
        _nameText.text = _name;
        _priceText.text = _price.ToString("N0") + "C";
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickEventHandler);
    }

    public void SetSoldOut()
    {
        GetComponent<Button>().interactable = false;
        _image.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
    }
}
