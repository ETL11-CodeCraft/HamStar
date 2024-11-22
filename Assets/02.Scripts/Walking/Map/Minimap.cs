using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// GPS ��� �̴ϸ� Ŭ����
/// </summary>
public class Minimap : MonoBehaviour
{
    private IGPS _gps => _unit.gps;


    private IUnitOfMinimap _unit;
    float _zoom = 17;   //�⺻���� �ҷ��� ���� zoom ��
    Vector2 _size = new Vector2(640f, 640f) * 3;
    //���⼭ size�� �ҷ����� �̹����� ������(20��ŭ zoom�� �� �̹����� ũ��, size�� Ŀ�� ���� �̹����� Ŀ���� ������ Ŀ���� �ʴ´�.)
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;

    private float _zoomin = 20; //zoom in�� �÷��̾ ���� �� ���� �ҷ��� zoom ��
    private bool _isPinching = false;
    [SerializeField] InputActionReference _twoFingerGesture;
    [SerializeField] RawImage _zoomMap;



    private void Awake()
    {
        _googleMapInterface = new GameObject("GoogleMapInterface").AddComponent<GoogleMapInterface>();
        _map.rectTransform.sizeDelta = _size;   //���� �ҷ����� ũ��� ���� ���� �̹��� ũ�⸦ ����ȭ

        _twoFingerGesture.action.performed += ZoomMap;
    }

    private void Start()
    {
#if UNITY_EDITOR
        _unit = new MockUnitOfMinimap();    //event invoke�� �� ���� �����Ƿ� loadMap�� start�������� ó���Ѵ�.
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

    // ���� �ҷ��� �� �̹����� ����, �浵�� ���� �̵����� ������ ��ġ�� �� �̹����� ���ߴ� �Լ�
    private void MoveMap(float latitudeMap, float longitudeMap, float latitude, float longitude)
    {
        if (DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) < 200f || DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) < 200f)
        {
            //������ y������ �̵�. ������ +�� �̹����� - �������� �̵�
            if (latitude - latitudeMap > 0)
            {
                _map.transform.localPosition = new Vector3(0, (float)(-DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0);
            }
            else
            {
                _map.transform.localPosition = new Vector3(0, (float)(DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f)), 0);
            }

            //�浵�� x������ �̵�. �浵�� +�� �̹����� - �������� �̵�
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
            //�Ÿ��� 200m�̻� �־����� ���� ���� �ҷ��´ٸ� �̹����� ��ġ�� Vector3.zero�� �ʱ�ȭ
            _map.transform.localPosition = Vector3.zero;
        }
    }

    private void ZoomMap(InputAction.CallbackContext context)
    {
        Debug.Log("�� �հ��� ������");

        // �Է��� ���� ��� ó��
        if (!context.ReadValueAsButton())
        {
            Debug.Log("Ȯ���� �̹��� ��Ȱ��ȭ");
            if (_isPinching && _zoomMap != null)
            {
                _zoomMap.gameObject.SetActive(false); // �̹��� ��Ȱ��ȭ
                _isPinching = false;
            }
            return;
        }

        // ���� ��ġ �Է� �б�
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch1 = Touchscreen.current.touches[0];
            var touch2 = Touchscreen.current.touches[1];

            // �� �հ����� Ȱ�� �������� Ȯ��
            if (touch1.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Ended &&
                touch2.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Vector2 touch1Start = touch1.startPosition.ReadValue();
                Vector2 touch2Start = touch2.startPosition.ReadValue();
                Vector2 pinchStartPosition = (touch1Start + touch2Start) / 2; // ��ġ ���� �߽�

                // ��ġ ���� ��ġ�� ��� ������Ʈ ������ Ȯ��
                if (RectTransformUtility.RectangleContainsScreenPoint(_map.rectTransform, pinchStartPosition, Camera.main))
                {
                    if (!_isPinching)
                    {
                        _isPinching = true;

                        // ���ο� �̹��� Ȱ��ȭ
                        if (_zoomMap != null)
                        {
                            Debug.Log("Ȯ���� �̹��� Ȱ��ȭ");
                            _googleMapInterface.LoadMap(_gps.latitude, _gps.longitude, _zoomin, _size, (texture) => _zoomMap.texture = texture);
                            _zoomMap.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}