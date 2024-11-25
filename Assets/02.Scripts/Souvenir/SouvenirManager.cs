using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SouvenirManager : MonoBehaviour
{
    [SerializeField] private List<SouvenirSpec> _souvenirList = new List<SouvenirSpec>(8);
    private Vector2 _origin = new Vector2(-140f, 500f);
    private float _screenWidth;
    private const float DELTA_X = 280f;
    private const float DELTA_Y = -230f;
    private const int ROW = 4;
    private const int COL = 2;
    [SerializeField] private Souvenir _souvenirPrefab;
    private Dictionary<int,Souvenir> _souvenirItems = new Dictionary<int,Souvenir>(8);
    private List<int> _uncollectedItems = new List<int>(8);
    [SerializeField] SouvenirInfo _souvenirInfo;
    private int _pageIdx = 0;
    private float _panelOrigin;
    private DataLoader _dataLoader;
    private SouvenirData _souvenirData;

    [SerializeField] private GameObject _souvenirPopup;

    [SerializeField] private Transform _canvasTransform;
    private SwipeControls _swipeControls;
    private float _minimumSwipeMagnitude = 10f;
    private Vector2 _swipeDir;
    private float _canvasOrigin;
    private int _canvasIdx = 0;
    public Action travelRefreshAction;


    private void Awake()
    {
        _dataLoader = new DataLoader();
        _souvenirData = _dataLoader.Load<SouvenirData>();
    }

    private void Start()
    {
        _swipeControls = new SwipeControls();
        _swipeControls.Player.Enable();
        _swipeControls.Player.Touch.canceled += ProcessTouchComplete;
        _swipeControls.Player.Swipe.performed += ProcessSwipeDelta;

        _screenWidth = Screen.width;
        _panelOrigin = transform.position.x;
        _canvasOrigin = _canvasTransform.position.x;

        for(int i=0;i<_souvenirList.Count;i++)
        {
            var obj = Instantiate(_souvenirPrefab);
            var curItem = _souvenirList[i];

            //기본 Info 설정
            obj.SetSouvenir(curItem.SouvenirName, curItem.SouvenirSprite, curItem.SouvenirDescription, curItem.SouvenirID);

            //위치 설정
            obj.transform.SetParent(transform);
            obj.transform.localPosition = _origin + new Vector2(((i % COL == 0) ? 0 : DELTA_X) + _screenWidth * (i / (ROW*COL)), DELTA_Y * ((i / COL) % ROW));

            //OnClickAction 설정
            obj.onClickAction += _souvenirInfo.ActiveInfo;

            //추후 획득한 기념품을 쉽게 찾을 수 있도록 Dictionary 사용
            if(!_souvenirItems.ContainsKey(curItem.SouvenirID))
            {
                _souvenirItems.Add(curItem.SouvenirID, obj);
                if(!_souvenirData.collectedSouvenir.Contains(curItem.SouvenirID))
                {
                    _uncollectedItems.Add(curItem.SouvenirID);
                }
                else
                {
                    obj.IsCollected = true;
                }
            }
        }
    }

    public void ProcessTouchComplete(InputAction.CallbackContext context)
    {
        Debug.LogWarning($"Swipe Magnitude : {_swipeDir.magnitude}");
        if (Mathf.Abs(_swipeDir.magnitude) < _minimumSwipeMagnitude) return;

        travelRefreshAction?.Invoke();

        if(_swipeDir.x > 0)
        {
            Debug.LogWarning("SWIPE RIGHT");
            if (_canvasIdx <= -1) return;

            _canvasIdx--;
            _canvasTransform.DOMoveX(_canvasOrigin - _canvasIdx * _screenWidth, 0.5f);
        }
        else if(_swipeDir.x < 0)
        {
            Debug.LogWarning("SWIPE LEFT");
            if (_canvasIdx >= 1) return;

            _canvasIdx++;
            _canvasTransform.DOMoveX(_canvasOrigin - _canvasIdx * _screenWidth, 0.5f);
        }
    }
    public void ProcessSwipeDelta(InputAction.CallbackContext context)
    {
        _swipeDir = context.ReadValue<Vector2>();
    }

    
    public void MoveNextPage()
    {
        if (_pageIdx >= _souvenirList.Count / (COL * ROW)) return;

        _pageIdx++;
        transform.DOMoveX(_panelOrigin - _pageIdx * _screenWidth, 0.5f);
    }
    public void MovePrevPage()
    {
        if (_pageIdx <= 0) return;

        _pageIdx--;
        transform.DOMoveX(_panelOrigin - _pageIdx * _screenWidth, 0.5f);
    }

    public void CollectSouvenir()
    {
        _souvenirPopup.SetActive(false);

        if(_uncollectedItems.Count <= 0)
        {
            //이미 모든 기념품을 획득하였습니다.
            return;
        }
        var ItemId = _uncollectedItems[UnityEngine.Random.Range(0,_uncollectedItems.Count)];

        _souvenirItems[ItemId].IsCollected = true;
        _uncollectedItems.Remove(ItemId);

        _souvenirData.collectedSouvenir.Add(ItemId);
        _dataLoader.Save(_souvenirData);

        if (_uncollectedItems.Count <= 0)
        {
            //축하합니다. 모든 기념품을 획득하였습니다.
        }
    }

    public void CloseSouvenirPopup()
    {
        _souvenirPopup.SetActive(false);
    }
}
