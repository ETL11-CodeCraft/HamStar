using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

/// <summary>
/// ���� ���� ����, ������ �ϰ� ���� ���� �׷����� ��Ÿ����.
/// ���� ���� ���� ���������� ���� ���ϰ�, �������ڸ� �����ϸ� ��� ������ ������ �������� ��ũ��Ʈ
/// </summary>
public class StepCount : MonoBehaviour, IStepCount
{
    public int stepCount => _stepCount;

    public int[] thisWeekStepCount => _thisWeekStepCount;

    public int coinChestCount => _chestCount;

    public int firstStepCount => _firstStepCount;

    private int _firstStepCount;
    private int _stepCount;
    private int[] _thisWeekStepCount = new int[7];
    private int _chestCount;

    private void Awake()
    {
#if UNITY_ANDROID
        AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
#elif UNITY_EDITOR

#endif
    }

    void Start()
    {
#if UNITY_ANDROID
        InputSystem.EnableDevice(AndroidStepCounter.current);
        AndroidStepCounter.current.MakeCurrent();
        //SaveManager.LoadWalkData();
#elif UNITY_EDITOR

#endif
    }


    void ChestOpen()    //���� ������ �ִ� coinChestCount��ŭ ��� ����
    {
        if(coinChestCount > 0)
        {
            for(int i = 0;  i < coinChestCount; i++)
            {
                int randomCoin = Random.Range(80, 120); //�����Լ� �׷����� �����ұ�.(80 ~ 120)�� 95%�� 50 ~ 200 �Լ�
                //coinText.text = randomCoin.ToString();
                GameManager.coin += randomCoin;
            }
        }
    }

}