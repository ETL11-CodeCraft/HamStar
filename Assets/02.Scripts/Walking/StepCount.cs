using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

public class StepCount : MonoBehaviour
{
    [SerializeField] public Text text;
    private bool isFirstPlay = true;
    private int firstStepCount = 0;

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
        if(isFirstPlay)
        {
            firstStepCount = AndroidStepCounter.current.stepCounter.ReadValue();
            isFirstPlay = false;
        }
#elif UNITY_EDITOR

#endif
    }

    void Update()
    {
#if UNITY_ANDROID
        text.text = (AndroidStepCounter.current.stepCounter.ReadValue() - firstStepCount).ToString();
#elif UNITY_EDITOR

#endif
    }

}