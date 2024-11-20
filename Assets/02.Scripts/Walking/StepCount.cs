using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

/// <summary>
/// 걸음 수를 세고, 금주의 일간 걸음 수를 그래프로 나타낸다.
/// 걸음 수를 토대로 보물상자의 수를 더하고, 보물상자를 오픈하면 모두 열리고 코인이 더해지는 스크립트
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

    public void ResetChestCount()   //안된다면 보물상자 하나 열릴 때마다 하나씩 더하기
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