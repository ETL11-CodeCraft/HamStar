using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

/// <summary>
/// 메인 화면에서 왼쪽으로 스와이프 했을 때, update구문 재생
/// </summary>
public class PlayerWalk : MonoBehaviour
{
    private IGPS _gps => _unit.gps;


    private IUnitOfMinimap _unit;
    [SerializeField] float _zoom = 4;
    [SerializeField] Vector2 _size = new Vector2(512f, 512f);
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;

    private IStepCount _stepCount;
    [SerializeField] public Text walkCountText;


    private void Awake()
    {
        _googleMapInterface = new GameObject("GoogleMapInterface").AddComponent<GoogleMapInterface>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        _unit = new MockUnitOfMinimap();
#elif UNITY_ANDROID
            _unit = new UnitOfMinimap();
#endif
        RefeshMap();
    }

    private void RefeshMap()
    {
        _googleMapInterface.LoadMap(_gps.latitude, _gps.longitude, _zoom, _size, (texture) => _map.texture = texture);
    }

    void StepCounter()
    {
#if UNITY_ANDROID
        walkCountText.text = (AndroidStepCounter.current.stepCounter.ReadValue() - _stepCount.firstStepCount).ToString();
#elif UNITY_EDITOR

#endif
    }
}
