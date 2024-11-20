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
    private int _currentChestCount = 0;
    private int _pastChestCount = 0;
    [SerializeField] Image _coinImage;
    [SerializeField] Transform _coinContent;
    //private List<GameObject> _coinPool;
    [SerializeField] GameObject _chestOpenBackGround;

    private void Awake()
    {
#if UNITY_ANDROID
        AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
#elif UNITY_EDITOR

#endif
    }

    void Start()
    {
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
        int readWalkCount = AndroidStepCounter.current.stepCounter.ReadValue();
        _currentChestCount = (readWalkCount / 1000) - _pastChestCount;
        chestCountText.text = _currentChestCount.ToString();
#elif UNITY_EDITOR

#endif
    }

    public void ChestOpen()    //현재 가지고 있는 coinChestCount만큼 모두 오픈
    {
        Debug.Log("ChestOpen");

        //_coinPool = new List<GameObject>();
        //_coinPool.Clear();

        if (_currentChestCount > 0)
        {
            for (int i = 0; i < _currentChestCount; i++)
            {
                int randomCoin = UnityEngine.Random.Range(80, 120); //정규함수 그래프로 변경할까.(80 ~ 120)이 95%인 50 ~ 200 함수
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
                //_coinPool.Add(image.gameObject);
            }

            ResetChestCount();
        }

        _chestOpenBackGround.SetActive(true);
    }

    public void TouchChestOpenBackGround()
    {
        _chestOpenBackGround.SetActive(false);
    }

    /// <summary>
    /// 보물 상자를 오픈하고나면, pastChestCount를 조정하고, 상자의 갯수가 0에서 시작하도록 하는 함수
    /// </summary>
    void ResetChestCount()
    {
        Transform[] childList = _coinContent.GetComponentsInChildren<Transform>();

        if(childList != null)
        {
            for(int i = 0;i < childList.Length;i++)
            {
                if(childList[i] != transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }
    }

    //값이 저장이 되지 않을 때 사용할 함수
    void PastChestCountSet()
    {
        while(_currentChestCount > _pastChestCount)
        {
            _pastChestCount++;
            if (_currentChestCount == _pastChestCount)
                break;
        }
    }
}