using TMPro;
using UnityEngine;
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
    //���� static api�� �ִ� ũ��� 640 * 640�̸�, �� �̻��� ũ��� �����ͷ� �����ؾ� �Ѵ�.
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;
    [SerializeField] TextMeshProUGUI _text;

    float _zoomin = 20; //zoom in�� �÷��̾ ���� �� ���� �ҷ��� zoom ��


    private void Awake()
    {
        _googleMapInterface = new GameObject("GoogleMapInterface").AddComponent<GoogleMapInterface>();
        _map.rectTransform.sizeDelta = _size;   //���� �ҷ����� ũ��� ���� ���� �̹��� ũ�⸦ ����ȭ
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
    }

    private void RefreshMap(float latitude, float longitude)
    {
        //_gps.latitude;
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
                _map.transform.position = new Vector3(0, -DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f), 0);
            }
            else
            {
                _map.transform.position = new Vector3(0, DistanceCalculator.GetDistance(latitude, longitudeMap, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f), 0);
            }

            //�浵�� x������ �̵�. �浵�� +�� �̹����� - �������� �̵�
            if (longitude - longitudeMap > 0)
            {
                _map.transform.position = new Vector3(-DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f), 0, 0);
            }
            else
            {
                _map.transform.position = new Vector3(DistanceCalculator.GetDistance(latitudeMap, longitude, latitudeMap, longitudeMap) / (DistanceCalculator.MeterPerPixelwithZoom(_zoom, latitude) / 3f), 0, 0);
            }
        }
        else
        {
            //�Ÿ��� 200m�̻� �־����� ���� ���� �ҷ��´ٸ� �̹����� ��ġ�� Vector3.zero�� �ʱ�ȭ
            //_map.transform.position = Vector3.zero;
        }
        Debug.Log("MoveMAP");
    }
}