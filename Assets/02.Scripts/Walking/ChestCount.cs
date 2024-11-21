using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

//�������� �� ����
public class ChestCount : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI chestCountText;
    [SerializeField] public TextMeshProUGUI stepCountTestText;
    [SerializeField] public TextMeshProUGUI pastChestCountTestText;
    private int _currentChestCount = 0;
    private int _pastChestCount = -1;
    [SerializeField] Image _coinImage;
    [SerializeField] Transform _coinContent;
    [SerializeField] GameObject _chestOpenBackGround;
    [SerializeField] Button _chestOpenButton;
    [SerializeField] Button _chestCloseButton;
    private DataLoader _dataLoader;
    private WalkData _walkData;
    private int _readWalkCount;
    private const int chestStepUnit = 1000;   //���� ������ �������� �ϳ�

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _walkData = _dataLoader.Load<WalkData>();
        _pastChestCount = _walkData.pastChestCount;
        _chestOpenButton.onClick.AddListener(ChestOpen);
        _chestCloseButton.onClick.AddListener(TouchChestOpenBackGround);

#if UNITY_EDITOR

#elif UNITY_ANDROID
        StartCoroutine(PermissionActivity("android.permission.ACTIVITY_RECOGNITION"));
#endif
    }

    void Start()
    {
        _chestOpenBackGround.SetActive(false);

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
        chestCountText.text = _currentChestCount.ToString();
        pastChestCountTestText.text = "pastChestCount : " + _pastChestCount.ToString();
#elif UNITY_ANDROID              
        _readWalkCount = AndroidStepCounter.current.stepCounter.ReadValue();
        if(_readWalkCount != 0)
        {
            if(_pastChestCount == -1)
            {
                _pastChestCount = (_readWalkCount / chestStepUnit);
                _walkData.pastChestCount = _pastChestCount;
                _dataLoader.Save<WalkData>(_walkData);
                _walkData = _dataLoader.Load<WalkData>();
            }
            else
            {
                stepCountTestText.text = "stepCount : " + _readWalkCount.ToString();
                _currentChestCount = (_readWalkCount / chestStepUnit) - _pastChestCount; 

                chestCountText.text = Mathf.Min(9, _currentChestCount).ToString();
                pastChestCountTestText.text = "pastChestCount : " + _pastChestCount.ToString();

                if (_readWalkCount / chestStepUnit < _pastChestCount)  
                {
                    _pastChestCount = 0;
                    Debug.Log($"_currentChestCount < 0 {_currentChestCount}");
                }
            }
        }
        else
        {
            stepCountTestText.text = "stepCount : 0";
            chestCountText.text = "0";
            pastChestCountTestText.text = "pastChestCount : 0";
        }
#endif
    }

    private IEnumerator PermissionActivity(string permission)
    {
        // ������ ������ �ʾҴٸ�
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            // �ֽ� Unity API�� ����� ������ ��û
            Permission.RequestUserPermission(permission);

            // ���� ���� ������ ��ٸ�
            while (!Permission.HasUserAuthorizedPermission(permission))
            {
                yield return null;
            }
        }
    }


    private void ChestOpen()    //���� ������ �ִ� coinChestCount��ŭ ��� ����(�ִ� 9��)
    {
        if (_currentChestCount > 0)
        {
            for (int i = 0; i < Mathf.Min(9, _currentChestCount); i++)
            {
                int randomCoin = UnityEngine.Random.Range(80, 120); //�����Լ� �׷����� �����ұ�.(80 ~ 120)�� 95%�� 50 ~ 200 �Լ�
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
            }
            _pastChestCount += _currentChestCount;
            _chestOpenBackGround.SetActive(true);
            _walkData.pastChestCount = _pastChestCount;
            _dataLoader.Save<WalkData>(_walkData);
            _walkData = _dataLoader.Load<WalkData>();
        }
    }

    public void TouchChestOpenBackGround()
    {
        ResetChestOpen();
        _chestOpenBackGround.SetActive(false);
    }

    /// <summary>
    /// ���� ���ڸ� �����ϰ���, pastChestCount�� �����ϰ�, ������ ������ 0���� �����ϵ��� �ϴ� �Լ�
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
//save data�� pastChestCount == null�̸� pastChestCount�� �ʱⰪ�� �����Ѵ�.
//currentChestCount�� ������� pastChestCount = 0���� �ʱ�ȭ�Ѵ�.