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
        _chestCloseButton.onClick.AddListener(ChestClose);

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
#elif UNITY_ANDROID              
        _readWalkCount = AndroidStepCounter.current.stepCounter.ReadValue();

        //AndroidStepCounter.current.stepCounter.ReadValue()�� ���� �ҷ����� ������ ��, 0�� �Ǵ� �κ��� ����ó��
        if(_readWalkCount != 0)
        {
            //����� �����Ͱ� ������(ó�� �÷��̽�), 1000������ ���λ����� ���� 0���� �ʱ�ȭ�ϱ� ���� �ȵ���̵忡 ����Ǿ��ִ� ���� ���� ���� _currentChestCount == 0�� �Ǵ� _pastChestCount�� �����Ѵ�.
            if(_pastChestCount == -1)
            {
                _pastChestCount = (_readWalkCount / chestStepUnit);
                _walkData.pastChestCount = _pastChestCount;
                _dataLoader.Save<WalkData>(_walkData);
                _walkData = _dataLoader.Load<WalkData>();
            }
            else
            {
                _currentChestCount = (_readWalkCount / chestStepUnit) - _pastChestCount; 

                chestCountText.text = Mathf.Min(9, _currentChestCount).ToString();

                //�ȵ���̵� ����� ������ �ٽ� ������ ��, AndroidStepCounter.current.stepCounter.ReadValue()�� 0�� �Ǵ� ���� ����Ͽ�, _pastChestCount = 0���� �ʱ�ȭ�Ѵ�.
                if (_readWalkCount / chestStepUnit < _pastChestCount)  
                {
                    _pastChestCount = 0;
                }
            }
        }
        else
        {
            chestCountText.text = "0";
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

    //���� ���� ������ ����ŭ ������ �Լ�(��ư) (�ִ� 9��)
    private void ChestOpen()
    {
        if (_currentChestCount > 0)
        {
            for (int i = 0; i < Mathf.Min(9, _currentChestCount); i++)
            {
                int randomCoin = UnityEngine.Random.Range(80, 120);
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
            }
            _pastChestCount += _currentChestCount;
            _chestOpenBackGround.SetActive(true);

            //���� ���� ���ڰ� �ٽ� ������ �ʵ��� _pastChestCount�� ����
            _walkData.pastChestCount = _pastChestCount;
            _dataLoader.Save<WalkData>(_walkData);
        }
    }

    //������ ���� ui�� �ݴ� �Լ�(��ư)
    public void ChestClose()
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
