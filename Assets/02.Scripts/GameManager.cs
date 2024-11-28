using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int coin = 0;
    public bool cantSwipe = false;

    //Text
    public TextMeshProUGUI _seedCount;
    public TextMeshProUGUI _goldseedCount;
    public TextMeshProUGUI _potionCount;

    private DataLoader _dataLoader;
    private InventoryData _inventoryData;
    private PlacementData _placementData;
    [SerializeField] ProductData _productData;
    private HamsterWheel _wheel;

    private PotionManager _potionManager;
    private FeedingManager _feedingManager;


    public HamsterWheel Wheel { 
        get
        {
            return _wheel;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        RefreshInventoryData();
        RefreshPlacementData();

        _potionManager = FindObjectOfType<PotionManager>();
        _feedingManager = FindObjectOfType<FeedingManager>();
    }

    public void RefreshInventoryData()
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        coin = _inventoryData.coin;
        //_seedCount.text = $"{GetQuantityForId(0)}";
        //_goldseedCount.text = $"{GetQuantityForId(1)}";
        //_potionCount.text = $"{GetQuantityForId(2)}";
    }

    private int GetQuantityForId(int id)
    {
        var item = _inventoryData.quantityForProductId.Find((x) => x.productId == id);
        return item.quantity;
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

    public void UpdateCountUI(int productID, TextMeshProUGUI uiText)  //아이템 개수 UI 업데이트
    {
        _inventoryData = _dataLoader.Load<InventoryData>();
        InventoryItem? item = _inventoryData.quantityForProductId.Find(v => v.productId == productID);

        if (item != null)
        {
            uiText.text = item.Value.quantity.ToString();
            if (item.Value.productId == 10001 && item.Value.quantity > 0)
            {
                _feedingManager?.SetSeedBtnInteractable(true);
                _feedingManager.SetSeedEventTriggerActive(true);
            }

            // 씨앗 개수에 따라 버튼 및 이벤트 트리거 비활성화
            if (item.Value.productId == 10001) // 씨앗 ID
            {
                bool isInteractable = item.Value.quantity > 0;
                _feedingManager?.SetSeedBtnInteractable(isInteractable);
                _feedingManager?.SetSeedEventTriggerActive(isInteractable);
            }
            else if (item.Value.productId == 10002) // 골드 씨앗 ID
            {
                bool isInteractable = item.Value.quantity > 0;
                _feedingManager?.SetGoldSeedBtnInteractable(isInteractable);
                _feedingManager?.SetGoldSeedEvnetTriggerActive(isInteractable);
            }
            else if (item.Value.productId == 10003)  //물약 ID
            {
                bool isInteractable = item.Value.quantity > 0;
                _potionManager?.SetPotionEventTriggerActive(isInteractable);
                _potionManager?.SetPotionEventTriggerActive(isInteractable);

            }
        }
        else
        {
            uiText.text = "0";
        }
    }

    public void ReduceCount(int productId)   //개수 하나씩 줄어들도록 하는 함수
    {
        var list = _inventoryData.quantityForProductId;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].productId == productId)
            {
                var quantity = list[i].quantity;
                _inventoryData.quantityForProductId.Remove(list[i]);

                InventoryItem availableItem = new InventoryItem();
                availableItem.productId = productId;
                availableItem.quantity = Mathf.Max(quantity - 1,0);//0이하로 감소하지 않도록

                _inventoryData.quantityForProductId.Insert(i, availableItem);

                _dataLoader.Save(_inventoryData);

                return;
            }
        }
    }
}
