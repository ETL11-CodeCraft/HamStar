using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PotionManager : MonoBehaviour
{
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] Camera _xrCamera;

    [SerializeField] GameObject _potionPrefab;  //물약 프리팹
    [SerializeField] Button _potionBtn; //물약 버튼
    [SerializeField] GameObject _potionPanel; //물약 설명 패널 
    [SerializeField] EventTrigger _potionBtnTrigger;
    //Image
    private RectTransform _rectTransform;
    [SerializeField] Image _potionImg;

    GameObject _spawnObject;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private DataLoader _dataLoader;
    private InventoryData _inventoryData;

    private void Awake()
    {
        _dataLoader = new DataLoader();

        _potionPanel.SetActive(false);  // Potion Instruction panel
        _potionImg.gameObject.SetActive(false);  //Potion drag image

        // Potion PointerDown 이벤트 트리거 
        _rectTransform = _potionImg.GetComponent<RectTransform>();

        EventTrigger.Entry onPointerDownEntry_Potion = new EventTrigger.Entry();
        onPointerDownEntry_Potion.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry_Potion.callback.AddListener(eventData =>
        {
            _inventoryData = _dataLoader.Load<InventoryData>();
            if (_inventoryData.quantityForProductId.Find(v => v.productId == 10003).productId == 0) return;

            Vector2 TouchPosition = ((PointerEventData)eventData).position;

            _potionImg.gameObject.SetActive(true);
            _rectTransform.position = TouchPosition;

            _potionPanel.SetActive(true);
            GameManager.instance.ReduceCount(10003);

        });
        _potionBtnTrigger.triggers.Add(onPointerDownEntry_Potion);

        //Potion Drag 이벤트 트리거 
        EventTrigger.Entry onPointerDragEntry_potion = new EventTrigger.Entry();
        onPointerDragEntry_potion.eventID = EventTriggerType.Drag;
        onPointerDragEntry_potion.callback.AddListener(eventData =>
        {
            Debug.Log("OnDrag");
            
            if (_inventoryData.quantityForProductId.Find(v => v.productId == 10003).productId == 0) return;

            // 터치 포지션을 가져온다
            Vector2 touchPosition = ((PointerEventData)eventData).position;
            _rectTransform.position = touchPosition;
        });

        _potionBtnTrigger.triggers.Add(onPointerDragEntry_potion);

        //Potion - PointerUp 이벤트 트리거
        EventTrigger.Entry OnPointerUpEntry_potion = new EventTrigger.Entry();
        OnPointerUpEntry_potion.eventID = EventTriggerType.PointerUp;
        OnPointerUpEntry_potion.callback.AddListener(eventData =>
        {
            if (_inventoryData.quantityForProductId.Find(v => v.productId == 10003).productId == 0) return;

            _potionImg.gameObject.SetActive(false);

            Vector2 TouchPosition = ((PointerEventData)eventData).position;
            Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);

            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                Pose hitPose = _hits[0].pose;

                Vector3 spawnPosition = hitPose.position + new Vector3(0, 0.1f, 0);
                _spawnObject = Instantiate(_potionPrefab, spawnPosition, Quaternion.Euler(-58f, -131f, -34f));
                _spawnObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                Debug.Log("No AR Plane found for seed placement");
            }

            _potionPanel.SetActive(false);
            GameManager.instance.UpdateCountUI(10003, GameManager.instance._potionCount);
            //GameManager.instance.RefreshInventoryData();

        });
        _potionBtnTrigger.triggers.Add(OnPointerUpEntry_potion);
    }
    private IEnumerator WaitForGameManager()
    {
        // GameManager.instance와 _potionCount가 초기화될 때까지 대기
        while (GameManager.instance == null || GameManager.instance._potionCount == null)
        {
            yield return null; // 다음 프레임까지 대기
        }

        Debug.Log("GameManager and _potionCount are ready!");

        yield return new WaitForSeconds(0.1f);

        GameManager.instance.UpdateCountUI(10003, GameManager.instance._potionCount);
    }

    private void Start()
    {
        StartCoroutine(WaitForGameManager());
    }

    public void SetPotionBtnInteractable(bool isInteractable)
    {
        if (_potionBtn != null)
        {
            _potionBtn.interactable = isInteractable;
        }
    }
    public void SetPotionEventTriggerActive(bool isActive)
    {
        if (_potionBtnTrigger != null)
        {
            _potionBtnTrigger.enabled = isActive;
        }
    }
}
