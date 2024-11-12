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
            new Item("����� ���(10��)", "������ ������ ���� �ż��� ����Դϴ�.\r\n�ܽ����� �������� 50��ŭ �ø��ϴ�.", Item.ItemType.Consumable),
            new Item("���� ���� Ʈ��(10��)", "�ٻ��ϰ� �ǰ��� �ܹ��� ����, �ܽ��Ͱ� �Ա� ���� ũ���Դϴ�." +
                "\r\n�ܽ����� �������� 50��ŭ �ø���, ģ�е��� 5��ŭ �ø��ϴ�.", Item.ItemType.Consumable),
            new Item("ġ�� ����", "��ȭ ������ �ܽ��͸� ġ���� �� �ֽ��ϴ�.\r\n(��� �� ��Ʈ���� ��ġ�� 0�� �˴ϴ�.", Item.ItemType.Consumable),
            new Item("�¹���", "�ܽ��Ͱ� ��� �� �ֽ��ϴ�.\r\n��Ʈ���� ���� �ӵ��� �����մϴ�.", Item.ItemType.Consumable, new Vector3(0.10f, 0.15f, 0.08f))
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
