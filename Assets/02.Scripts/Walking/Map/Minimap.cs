using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// GPS 기반 미니맵 클래스
/// </summary>
public class Minimap : MonoBehaviour
{
    private IGPS _gps => _unit.gps;


    private IUnitOfMinimap _unit;
    float _zoom = 17;   //기본으로 불러올 맵의 zoom 값
    Vector2 _size = new Vector2(640f, 640f) * 3;
    //여기서 size는 불러오는 이미지의 사이즈(20만큼 zoom이 된 이미지의 크기, size가 커질 수록 이미지만 커지고 범위는 커지지 않는다.)
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;

    private float _zoomin = 20; //zoom in을 플레이어가 했을 때 맵을 불러올 zoom 값
    private bool _isPinching = false;
    [SerializeField] InputActionReference _twoFingerGesture;
    [SerializeField] RawImage _zoomMap;



    private void Awake()
    {
        _googleMapInterface = new GameObject("GoogleMapInterface").AddComponent<GoogleMapInterface>();
        _map.rectTransform.sizeDelta = _size;   //맵을 불러오는 크기와 맵을 넣을 이미지 크기를 동일화

        _twoFingerGesture.action.performed += ZoomMap;
    }

    private void Start()
    {
#if UNITY_EDITOR
        _unit = new MockUnitOfMinimap();    //event invoke를 할 일이 없으므로 loadMap을 start구문에서 처리한다.
        _googleMapInterface.LoadMap(_gps.latitude, _gps.longitude, _zoom, _size, (texture) => _map.texture = texture);
#elif UNITY_ANDROID
            _unit = new UnitOfMinimap();
#endif
        _gps.onLocationChanged += RefreshMap;
        _gps.onMapMoved += MoveMap;
        _zoomMap.gameObject.SetActive(false);
    }

    private void RefreshMap(float latitude, float longitude)
    {
        _googleMapInterface.LoadMap(latitude, longitude, _zoom, _size, (texture) => _map.texture = texture);
        Debug.Log("REFRESHMAP");
    }

    // 현재 불러온 맵 이미지를 위도, 경도에 맞춰 이동시켜 현재의 위치와 맵 이미지를 맞추는 함수
    private void MoveMap(float latitudeMap, float longitudeMap, float latitude, float longitude)
    {
        if (DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) < 200f || DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) < 200f)
        {
            //위도는 y축으로 이동. 위도가 +면 이미지는 - 방향으로 이동
            if (latitude - latitudeMap > 0)
            {
                _map.transform.localPosition = new Vector3(0, (float)(-DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0);
            }
            else
            {
                _map.transform.localPosition = new Vector3(0, (float)(DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0);
            }

            //경도는 x축으로 이동. 경도가 +면 이미지는 - 방향으로 이동
            if (longitude - longitudeMap > 0)
            {
                _map.transform.localPosition = new Vector3((float)(-DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0, 0);
            }
            else
            {
                _map.transform.localPosition = new Vector3((float)(DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0, 0);
            }
        }
        else
        {
            //거리가 200m이상 멀어져서 맵을 새로 불러온다면 이미지의 위치는 Vector3.zero로 초기화
            _map.transform.localPosition = Vector3.zero;
        }
    }

    private void ZoomMap(InputAction.CallbackContext context)
    {
        Debug.Log("두 손가락 제스쳐");

        // 입력이 끝난 경우 처리
        if (!context.ReadValueAsButton())
        {
            Debug.Log("확대한 이미지 비활성화");
            if (_isPinching && _zoomMap != null)
            {
                _zoomMap.gameObject.SetActive(false); // 이미지 비활성화
                _isPinching = false;
            }
            return;
        }

        // 현재 터치 입력 읽기
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch1 = Touchscreen.current.touches[0];
            var touch2 = Touchscreen.current.touches[1];

            // 두 손가락이 활성 상태인지 확인
            if (touch1.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Ended &&
                touch2.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Vector2 touch1Start = touch1.startPosition.ReadValue();
                Vector2 touch2Start = touch2.startPosition.ReadValue();
                Vector2 pinchStartPosition = (touch1Start + touch2Start) / 2; // 핀치 시작 중심

                // 터치 시작 위치가 대상 오브젝트 위인지 확인
                if (RectTransformUtility.RectangleContainsScreenPoint(_map.rectTransform, pinchStartPosition, Camera.main))
                {
                    if (!_isPinching)
                    {
                        _isPinching = true;

                        // 새로운 이미지 활성화
                        if (_zoomMap != null)
                        {
                            Debug.Log("확대한 이미지 활성화");
                            _googleMapInterface.LoadMap(_gps.latitude, _gps.longitude, _zoomin, _size, (texture) => _zoomMap.texture = texture);
                            _zoomMap.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}