using System.Collections;
using TMPro;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _foodText;
    [SerializeField] TextMeshProUGUI _medicineText;
    [SerializeField] ProductData _productData;

    [SerializeField] Hamster _hamsterPrefab;
    private Hamster _hamster;
    private HamsterWheel _wheel;

    private DataLoader _dataLoader;
    private InventoryData _inventoryData;
    private PlacementData _placementData;

    private void Awake()
    {
        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        RefreshInventoryData();
        RefreshPlacementData();
    }

    private void Update()
    {
    }

    public void RefreshInventoryData()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        GameManager.coin = _inventoryData.coin;
        _coinText.text = $"코인 ({GameManager.coin})";
        _foodText.text = $"일반 먹이 ({GetQuantityForId(0)})\n특수 먹이 ({GetQuantityForId(1)})";
        _medicineText.text = $"치료 물약 ({GetQuantityForId(2)})";
    }

    private int GetQuantityForId(int id)
    {
        var item = _inventoryData.quantityForProductId.Find((x) => x.productId == id);
        return item.quantity;
    }

    public void AddCoin()
    {
        GameManager.coin += 10;
        _inventoryData.coin = GameManager.coin;
        _dataLoader.Save(_inventoryData);
        RefreshInventoryData();
    }

    private void RefreshPlacementData()
    {
        // 쳇바퀴 배치 정보 로드
        _placementData = _dataLoader.Load<PlacementData>();

        _placementData.placements.ForEach(p =>
        {
            Product product = GetProduct(p.productId);
            _wheel = Instantiate(product.prefab, p.position, p.rotation).GetComponent<HamsterWheel>();
        });
    }

    private Product GetProduct(int id)
    {
        return _productData.list.Find((v) => v.id == id);
    }

    public void CreateHamster ()
    {
        if (_hamster != null) Destroy(_hamster);
        _hamster = Instantiate(_hamsterPrefab, _wheel.transform.position + Vector3.right, Quaternion.identity);
        _hamster.gameObject.SetActive(true);
        StartCoroutine(C_Test());
    }

    IEnumerator C_Test()
    {
        while (Vector3.Distance(_hamsterPrefab.transform.position, _wheel.transform.position) < 0.1f)
        {
            _hamsterPrefab.transform.Translate(_wheel.transform.position * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
