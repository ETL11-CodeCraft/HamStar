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

    //public int[] thisWeekStepCount => _thisWeekStepCount;

    public int coinChestCount => _coinChestCount;

    public int pastChestCount => _pastChestCount;

    //public int firstStepCount => _firstStepCount;

    //private int _firstStepCount;
    private int _stepCount;
    //private int[] _thisWeekStepCount = new int[7];
    private int _coinChestCount;
    private int _pastChestCount;

    private void Awake()
    {
        AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
    }

    void Start()
    {
        InputSystem.EnableDevice(AndroidStepCounter.current);
        AndroidStepCounter.current.MakeCurrent();
        AndroidStepCounter.current.stepCounter.Setup();
        _coinChestCount = AndroidStepCounter.current.stepCounter.ReadValue() / 1000 - _pastChestCount;
        //SaveManager.LoadWalkData();

    }

    public int GetChestCount()
    {
        _coinChestCount = AndroidStepCounter.current.stepCounter.ReadValue();
        Debug.Log($"ChestCount = {_coinChestCount}");
        Debug.Log($"StepCount = {AndroidStepCounter.current.stepCounter.ReadValue()}");
        return _coinChestCount - _pastChestCount;
    }

    public void ResetChestCount()   //�ȵȴٸ� �������� �ϳ� ���� ������ �ϳ��� ���ϱ�
    {
        if (_coinChestCount > _pastChestCount)
        {
            _pastChestCount += _coinChestCount;
        }
        else
        {
            _pastChestCount = 0;
        }
    }
}