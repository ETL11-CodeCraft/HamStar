using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private IGPS _gps => _unit.gps;


    private IUnitOfMinimap _unit;
    [SerializeField] float _zoom = 4;
    [SerializeField] Vector2 _size = new Vector2(512f, 512f);
    private GoogleMapInterface _googleMapInterface;
    [SerializeField] RawImage _map;


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
}
