using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

//�������� �� ����
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

    public void ChestOpen()    //���� ������ �ִ� coinChestCount��ŭ ��� ����
    {
        Debug.Log("ChestOpen");

        //_coinPool = new List<GameObject>();
        //_coinPool.Clear();

        if (_currentChestCount > 0)
        {
            for (int i = 0; i < _currentChestCount; i++)
            {
                int randomCoin = UnityEngine.Random.Range(80, 120); //�����Լ� �׷����� �����ұ�.(80 ~ 120)�� 95%�� 50 ~ 200 �Լ�
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
    /// ���� ���ڸ� �����ϰ���, pastChestCount�� �����ϰ�, ������ ������ 0���� �����ϵ��� �ϴ� �Լ�
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

    //���� ������ ���� ���� �� ����� �Լ�
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