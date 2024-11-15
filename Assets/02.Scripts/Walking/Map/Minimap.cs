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
        _text.text = $"latitude {_gps.latitude}, longitude {_gps.longitude}";
#elif UNITY_ANDROID
            _unit = new UnitOfMinimap();
#endif
        _gps.onLocationChanged += RefreshMap;
    }

    private void RefreshMap(float latitude, float longitude)
    {
        //_gps.latitude;
        _googleMapInterface.LoadMap(latitude, longitude, _zoom, _size, (texture) => _map.texture = texture);
        _text.text = $"latitude {latitude}, longitude {longitude}";
        Debug.Log("REFRESHMAP");
    }
}