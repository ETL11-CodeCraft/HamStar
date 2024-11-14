using TMPro;
using UnityEngine;
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
    //구글 static api의 최대 크기는 640 * 640이며, 그 이상의 크기는 고객센터로 문의해야 한다.
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;
    [SerializeField] TextMeshProUGUI _text;

    float _zoomin = 20; //zoom in을 플레이어가 했을 때 맵을 불러올 zoom 값


    private void Awake()
    {
        _googleMapInterface = new GameObject("GoogleMapInterface").AddComponent<GoogleMapInterface>();
        _map.rectTransform.sizeDelta = _size;   //맵을 불러오는 크기와 맵을 넣을 이미지 크기를 동일화
    }

    private void Start()
    {
#if UNITY_EDITOR
        _unit = new MockUnitOfMinimap();    //event invoke를 할 일이 없으므로 loadMap을 start구문에서 처리한다.
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