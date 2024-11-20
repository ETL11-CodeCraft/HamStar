using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 걸음 수와 관련된 클래스
/// </summary>
public class PlayerWalk : MonoBehaviour
{
    private IStepCount _stepCount;
    //[SerializeField] private TextMeshProUGUI walkCountText;
    [SerializeField] private TextMeshProUGUI chestCountText;
    [SerializeField] Image _coinImage;
    [SerializeField] Transform _coinContent;
    private List<GameObject> _coinPool;
    [SerializeField] GameObject _chestOpenBackGround;

    private void Awake()
    {
#if UNITY_ANDROID
        AndroidRuntimePermissions.RequestPermissions("android.permission.ACTIVITY_RECOGNITION");
#elif UNITY_EDITOR
#endif
    }

    private void Start()
    {
        _chestOpenBackGround.SetActive(false);

#if UNITY_EDITOR
        _stepCount = new MockStepCount();
#elif UNITY_ANDROID
            _stepCount = new StepCount();
            chestCountText.text = _stepCount.GetChestCount().ToString();
#endif
    }

//    void StepCounter()
//    {
//#if UNITY_ANDROID
//        //walkCountText.text = AndroidStepCounter.current.stepCounter.ReadValue().ToString();
//#elif UNITY_EDITOR

//#endif
//    }


    public void ChestOpen()    //현재 가지고 있는 coinChestCount만큼 모두 오픈
    {
        Debug.Log("ChestOpen");

        _coinPool = new List<GameObject>();
        _coinPool.Clear();

        if (_stepCount.coinChestCount > 0)
        {
            for (int i = 0; i < _stepCount.coinChestCount; i++)
            {
                int randomCoin = Random.Range(80, 120); //정규함수 그래프로 변경할까.(80 ~ 120)이 95%인 50 ~ 200 함수
                GameManager.coin += randomCoin;
                Image image = Instantiate(_coinImage, _coinContent);
                image.GetComponentInChildren<TextMeshProUGUI>().text = randomCoin.ToString();
                _coinPool.Add(image.gameObject);
            }

            _stepCount.ResetChestCount();
        }

        _chestOpenBackGround.SetActive(true);
    }

    public void TouchChestOpenBackGround()
    {
        _chestOpenBackGround.SetActive(false);
    }
}
