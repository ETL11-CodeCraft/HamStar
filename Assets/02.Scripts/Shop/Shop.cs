using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject _shopPanel;
    [SerializeField] GameObject _itemPanel;
    [SerializeField] GameObject _shopSuccessPanel;
    [SerializeField] GameObject _shopFailPanel;
    [SerializeField] GameObject _productPrefab;

    List<Item> _items;
    List<Product> _products;

    Product _currentProduct;

    public void OpenShop()
    {
        _shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        _shopPanel.SetActive(false);
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
        return null;
    }

    public bool Buy()
    {
        if (GameManager.coin <= _currentProduct.Price)
        {
            return false;
        }

        if (_currentProduct.Item.Type == Item.ItemType.Installation)
        {
            //
        }

        return true;
    }

    private void Awake()
    {
        _items = new List<Item>
        {
            new Item("햄찌밥 사료(10개)", "국내산 원물로 만든 신선한 사료입니다.\r\n햄스터의 포만감을 50만큼 올립니다.", Item.ItemType.Consumable),
            new Item("동결 건조 트릿(10개)", "바삭하고 건강한 단백질 간식, 햄스터가 먹기 좋은 크기입니다." +
                "\r\n햄스터의 포만감을 50만큼 올리며, 친밀도를 5만큼 올립니다.", Item.ItemType.Consumable),
            new Item("치료 물약", "흑화 상태의 햄스터를 치료할 수 있습니다.\r\n(사용 후 스트레스 수치가 0이 됩니다.", Item.ItemType.Consumable),
            new Item("쳇바퀴", "햄스터가 운동할 수 있습니다.\r\n스트레스 증가 속도가 감소합니다.", Item.ItemType.Consumable, new Vector3(0.10f, 0.15f, 0.08f))
        };
        _products = new List<Product>
        {
            new Product(0, null, _items[0], 100, -1),
            new Product(1, null, _items[1], 200, -1),
            new Product(2, null, _items[2], 500, -1),
            new Product(3, null, _items[3], 500, 1)
        };
    }

    private void Start()
    {
        for (int i = 0; i < _products.Count; i++)
        {

        }

        _shopPanel.SetActive(false);
    }
}
