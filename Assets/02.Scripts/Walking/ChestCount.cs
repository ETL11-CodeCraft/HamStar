using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

//보물상자 수 저장
public class ChestCount : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI chestCountText;
    [SerializeField] public TextMeshProUGUI stepCountTestText;
    [SerializeField] public TextMeshProUGUI pastChestCountTestText;
    private int _currentChestCount = 0;
    private int _pastChestCount = 0;
    [SerializeField] Image _coinImage;
    [SerializeField] Transform _coinContent;
    [SerializeField] GameObject _chestOpenBackGround;
    [SerializeField] Button _chestOpenButton;
    [SerializeField] Button _chestCloseButton;
    private DataLoader _dataLoader;
    private WalkData _walkData;
    private int _readWalkCount;
    private const int chestStepUnit = 1000;   //단위 걸음당 보물상자 하나

    private void Awake()
    {
        //_dataLoader = new DataLoader();
        //_walkData = _dataLoader.Load<WalkData>();
        _chestOpenButton.onClick.AddListener(ChestOpen);
        _chestCloseButton.onClick.AddListener(TouchChestOpenBackGround);

#if UNITY_ANDROID
        AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
#elif UNITY_EDITOR

#endif
    }

    void Start()
    {
        //if(_walkData.pastChestCount == null)
        //{
        //    ResetChestCountSet();
        //}
        _chestOpenBackGround.SetActive(false);

#if UNITY_ANDROID
        InputSystem.EnableDevice(AndroidStepCounter.current);
        AndroidStepCounter.current.MakeCurrent();
        AndroidStepCounter.current.stepCounter.Setup();
#elif UNITY_EDITOR

#endif
    }

    void Update()
    {
#if UNITY_ANDROID
        _readWalkCount = AndroidStepCounter.current.stepCounter.ReadValue(); //값 확인
        stepCountTestText.text = "stepCount : " + _readWalkCount.ToString();
        _currentChestCount = (_readWalkCount / chestStepUnit) - _pastChestCount;
        //_pastChestCount = _currentChestCount;   
        chestCountText.text = _currentChestCount.ToString();
        pastChestCountTestText.text = "pastChestCount : " + _pastChestCount.ToString();
#elif UNITY_EDITOR
        _currentChestCount = 2;
        _pastChestCount = 1;
#endif
    }

    private void ChestOpen()    //현재 가지고 있는 coinChestCount만큼 모두 오픈
    {
        Debug.Log($"ChestOpen : {_currentChestCount}개");

        if (_currentChestCount > 0)
        {
            Debug.Log($"ChestOpen : chestCount > 0");
            for (int i = 0; i < _currentChestCount; i++)
            {
                Debug.Log($"ChestOpen : {i}th coin");
                int randomCoin = UnityEngine.Random.Range(80, 120); //정규함수 그래프로 변경할까.(80 ~ 120)이 95%인 50 ~ 200 함수
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
            }
            _pastChestCount += _currentChestCount;
            _chestOpenBackGround.SetActive(true);
        }
    }

    public void TouchChestOpenBackGround()
    {
        ResetChestOpen();
        _chestOpenBackGround.SetActive(false);
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
//save data의 pastChestCount == null이면 pastChestCount의 초기값을 저장한다.
//currentChestCount가 음수라면 pastChestCount = 0으로 초기화한다.