using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HamsterWheelPlacementController : MonoBehaviour
{
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] InputActionReference _tapStartPosition;
    [SerializeField] InputActionReference _pressHoldAction;
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] Camera _xrCamera;
    [SerializeField] GameObject _placementPrefab;
    [SerializeField] GameObject _rotationPrefab;
    [SerializeField] LayerMask _targetLayer;
    
    private GameObject _hamsterWheelPrefab;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _currentHamsterWheel;
    private bool _isPlacementMode = false;
    private bool _isRotationMode = false;
    private bool _isOK;
    private Vector3 _center;

    public bool IsPlacementMode {
        get { return _isPlacementMode; }
        set { _isPlacementMode = value; }
    }

    public GameObject HamsterWheelPrefab
    {
        set { _hamsterWheelPrefab = value; }
    }

    private void Start()
    {
        _dragCurrentPosition.action.performed += OnDrag;
        _tapStartPosition.action.started += OnTap;
        _pressHoldAction.action.performed += OnLongTap;

        _center = new Vector2(_xrCamera.pixelWidth / 2, _xrCamera.pixelHeight / 2);
        
        _placementPrefab = Instantiate(_placementPrefab);
        _placementPrefab.SetActive(false);
        _rotationPrefab = Instantiate(_rotationPrefab);
        _rotationPrefab.SetActive(false);
    }

    private void Update()
    {
        _placementPrefab.SetActive(_isPlacementMode && !_isRotationMode);
        _rotationPrefab.SetActive(_isPlacementMode && _isRotationMode);

        if (_isPlacementMode && _currentHamsterWheel == null)
        {
            if (_arRaycastManager.Raycast(_center, _hits, TrackableType.Planes) && _hits.Count > 0)
            {
                _placementPrefab.transform.position = _hits[0].pose.position + _hits[0].pose.up * 0.01f;
                _placementPrefab.transform.rotation = _hits[0].pose.rotation;
            }
        } 
        else if (_isRotationMode && _currentHamsterWheel != null)
        {
            if (_arRaycastManager.Raycast(_center, _hits, TrackableType.Planes) && _hits.Count > 0)
            {
                _rotationPrefab.transform.position = _currentHamsterWheel.transform.position;
                _rotationPrefab.transform.rotation = _currentHamsterWheel.transform.rotation;
            }
        }
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (!_isPlacementMode) return;

        Vector2 dragPosition = context.ReadValue<Vector2>();
        Ray ray = _xrCamera.ScreenPointToRay(dragPosition);

        if (_isPlacementMode && !_isRotationMode && Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _targetLayer))
        {
            Debug.Log("OnDrag -> Hit info => " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject == _currentHamsterWheel)
            {
                if (_arRaycastManager.Raycast(dragPosition, _hits, TrackableType.Planes) && _hits.Count > 0)
                {
                    _currentHamsterWheel.transform.position = _hits[0].pose.position;
                    _currentHamsterWheel.transform.rotation = _hits[0].pose.rotation;

                    _placementPrefab.transform.position = _hits[0].pose.position + _hits[0].pose.up * 0.01f;
                    _placementPrefab.transform.rotation = _hits[0].pose.rotation;
                }
            }
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        if (!_isPlacementMode) return;
        Debug.Log($"OnTap => {context.time} {context.startTime} {context.duration}");

        Vector2 tapPosition = context.ReadValue<Vector2>();

        if (_currentHamsterWheel == null)
        {
            if (_arRaycastManager.Raycast(tapPosition, _hits, TrackableType.Planes) && _hits.Count > 0)
            {
                _currentHamsterWheel = Instantiate(_hamsterWheelPrefab, _hits[0].pose.position, Quaternion.identity);
                Debug.Log("ÃÂ¹ÙÄû »ý¼º");
            }
        }
        else
        {
            Ray ray = _xrCamera.ScreenPointToRay(tapPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _targetLayer))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject == _currentHamsterWheel)
                {
                    Debug.Log("ÃÂ¹ÙÄû Ã£À½");
                    _isPlacementMode = true;
                }
            }
        }
    }

    private void OnLongTap(InputAction.CallbackContext context)
    {
        if (!_isPlacementMode) return;
        Debug.Log($"OnLongTap => {context.time} {context.startTime} {context.duration}");
        _isRotationMode = !_isRotationMode;
    }

    private void SavePlacementData()
    {
        // ÃÂ¹ÙÄû ¹èÄ¡ Á¤º¸ ÀúÀå
    }
}
