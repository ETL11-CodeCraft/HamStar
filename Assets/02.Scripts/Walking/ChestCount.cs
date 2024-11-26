using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

//보물상자 수 저장
public class ChestCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _chestCountText;
    [SerializeField] TextMeshProUGUI _getCoinText;
    private int _currentChestCount = 0;
    private int _pastChestCount = -1;
    [SerializeField] Image _coinImage;
    [SerializeField] Transform _coinContent;
    [SerializeField] GameObject _chestOpenBackGround;
    [SerializeField] Button _chestOpenButton;
    [SerializeField] Button _chestCloseButton;
    private DataLoader _dataLoader;
    private WalkData _walkData;
    private InventoryData _inventoryData;
    private int _readWalkCount;
    private const int _chestStepUnit = 1000;   //단위 걸음(1,000)당 보물상자 하나
    private int _getCoin = 0;

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _walkData = _dataLoader.Load<WalkData>();
        _inventoryData = _dataLoader.Load<InventoryData>();
        _pastChestCount = _walkData.pastChestCount;
        _chestOpenButton.onClick.AddListener(ChestOpen);
        _chestCloseButton.onClick.AddListener(ChestClose);

#if UNITY_EDITOR

#elif UNITY_ANDROID
        StartCoroutine(PermissionActivity("android.permission.ACTIVITY_RECOGNITION"));
#endif
    }

    void Start()
    {
        _chestOpenBackGround.SetActive(false);
        _getCoinText.gameObject.SetActive(false);

#if UNITY_EDITOR
        _currentChestCount = 2;
        _pastChestCount = 1;
#elif UNITY_ANDROID
        InputSystem.EnableDevice(AndroidStepCounter.current);
        AndroidStepCounter.current.MakeCurrent();
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        _chestCountText.text = _currentChestCount.ToString();
#elif UNITY_ANDROID              
        _readWalkCount = AndroidStepCounter.current.stepCounter.ReadValue();

        //AndroidStepCounter.current.stepCounter.ReadValue()의 값을 불러오지 못했을 때, 0이 되는 부분의 예외처리
        if(_readWalkCount != 0)
        {
            //저장된 데이터가 없으면(처음 플레이시), 1000걸음당 코인상자의 수를 0으로 초기화하기 위해 안드로이드에 저장되어있는 걸음 수를 통해 _currentChestCount == 0이 되는 _pastChestCount를 저장한다.
            if(_pastChestCount == -1)
            {
                _pastChestCount = (_readWalkCount / _chestStepUnit);
                _walkData.pastChestCount = _pastChestCount;
                _dataLoader.Save<WalkData>(_walkData);
                _walkData = _dataLoader.Load<WalkData>();
            }
            else
            {
                _currentChestCount = (_readWalkCount / _chestStepUnit) - _pastChestCount; 

                _chestCountText.text = Mathf.Min(9, _currentChestCount).ToString();

                //안드로이드 기기의 전원을 다시 시작할 시, AndroidStepCounter.current.stepCounter.ReadValue()이 0이 되는 점을 고려하여, _pastChestCount = 0으로 초기화한다.
                if (_readWalkCount / _chestStepUnit < _pastChestCount)  
                {
                    _pastChestCount = 0;
                }
            }
        }
        else
        {
            _chestCountText.text = "0";
        }
#endif
    }

    private IEnumerator PermissionActivity(string permission)
    {
        // 권한이 허용되지 않았다면
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            // 최신 Unity API를 사용해 권한을 요청
            Permission.RequestUserPermission(permission);

            // 권한 허용될 때까지 기다림
            while (!Permission.HasUserAuthorizedPermission(permission))
            {
                yield return null;
            }
        }
    }

    //모인 코인 상자의 수만큼 오픈할 함수(버튼) (최대 9개)
    private void ChestOpen()
    {
        _getCoin = 0;

        if (_currentChestCount > 0)
        {
            SoundManager.instance.PlayButtonSound();
            _chestOpenButton.enabled = false;
            _chestOpenButton.gameObject.GetComponent<Image>().color = Color.gray;
            for (int i = 0; i < Mathf.Min(9, _currentChestCount); i++)
            {
                int randomCoin = UnityEngine.Random.Range(80, 120);
                _getCoin += randomCoin;
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
            }
            _pastChestCount += _currentChestCount;
            _chestOpenBackGround.SetActive(true);

            //열린 코인 상자가 다시 열리지 않도록 _pastChestCount에 저장
            _walkData.pastChestCount = _pastChestCount;
            _dataLoader.Save<WalkData>(_walkData);
            _inventoryData.coin = GameManager.coin;
            _dataLoader.Save<InventoryData>(_inventoryData);
        }
    }

    //오픈한 코인 ui를 닫는 함수(버튼)
    public void ChestClose()
    {
        SoundManager.instance.PlayButtonSound();
        ResetChestOpen();
        _chestOpenBackGround.SetActive(false);
        StartCoroutine(GetCoinAlarm());
    }

    IEnumerator GetCoinAlarm()
    {
        _getCoinText.text = $"{_getCoin:#,##0} 코인을\n얻었습니다.";
        _getCoinText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        _getCoinText.gameObject.SetActive(false);
        _chestOpenButton.enabled = true;
        _chestOpenButton.gameObject.GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// 보물 상자를 오픈하고나면, pastChestCount를 조정하고, 상자의 갯수가 0에서 시작하도록 하는 함수
    /// </summary>
    void ResetChestOpen()
    {
        Transform[] childList = _coinContent.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != _coinContent.transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }
    }
}
