using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 걸음 수와 관련된 클래스
/// </summary>
public class PlayerWalk : MonoBehaviour
{
    private IStepCount _stepCount;
    [SerializeField] public TextMeshProUGUI walkCountText;


    private void Awake()
    {

    }

    private void Start()
    {
#if UNITY_EDITOR
        _stepCount = new MockStepCount();
#elif UNITY_ANDROID
            _stepCount = new StepCount();
#endif
    }

    void StepCounter()
    {
#if UNITY_ANDROID
        walkCountText.text = (AndroidStepCounter.current.stepCounter.ReadValue() - _stepCount.firstStepCount).ToString();
#elif UNITY_EDITOR

#endif
    }
}
