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


    void ChestOpen()    //현재 가지고 있는 coinChestCount만큼 모두 오픈
    {
        if(coinChestCount > 0)
        {
            for(int i = 0;  i < coinChestCount; i++)
            {
                int randomCoin = Random.Range(80, 120); //정규함수 그래프로 변경할까.(80 ~ 120)이 95%인 50 ~ 200 함수
                //coinText.text = randomCoin.ToString();
                GameManager.coin += randomCoin;
            }
        }
    }

}