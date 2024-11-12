using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

/// <summary>
/// ���� ���� ����, ������ �ϰ� ���� ���� �׷����� ��Ÿ����.
/// ���� ���� ���� ���������� ���� ���ϰ�, �������ڸ� �����ϸ� ��� ������ ������ �������� ��ũ��Ʈ
/// </summary>
public class StepCount : MonoBehaviour
{
    [SerializeField] public Text walkCountText;
    private bool isFirstPlay = true;
    private int firstStepCount = 0;
    private int chestCount = 0;

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
        //if(isFirstPlay)   //ù �÷��̽ÿ� �ҷ��� ���� ���� ������ 0���� �����ϰ��Ѵ�.
        //{
        //    firstStepCount = AndroidStepCounter.current.stepCounter.ReadValue();
        //    isFirstPlay = false;
        //}
#elif UNITY_EDITOR

#endif
    }

    void Update()
    {
#if UNITY_ANDROID
        walkCountText.text = (AndroidStepCounter.current.stepCounter.ReadValue() - firstStepCount).ToString();
#elif UNITY_EDITOR

#endif
    }

    void ChestOpen()
    {
        if(chestCount > 0)
        {
            for(int i = 0;  i < chestCount; i++)
            {
                int randomCoin = Random.Range(80, 120);
                //coinText.text = randomCoin.ToString();
                //GameManager.coint += randomCoin;
            }
        }
    }

}