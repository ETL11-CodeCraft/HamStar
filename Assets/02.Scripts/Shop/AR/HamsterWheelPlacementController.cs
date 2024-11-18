﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HamsterWheelPlacementController : MonoBehaviour
{
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] InputActionReference _dragDelta;
    [SerializeField] InputActionReference _tapStartPosition;
    [SerializeField] InputActionReference _pressHoldAction;
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] Camera _xrCamera;
    [SerializeField] GameObject _placementPrefab;
    [SerializeField] GameObject _rotationPrefab;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] GameObject _buttonUIPanel;
    [SerializeField] ARAnchorManager _arAnchorManager;

    public UnityAction CancelAction;
    public UnityAction<Product> ApplyAction;

    private GameObject _hamsterWheelPrefab;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _currentHamsterWheel;
    private bool _isPlacementMode = false;
    private bool _isRotationMode = false;
    private Vector3 _center;
    private bool _isDragging;
    private Product _product;

    public bool IsPlacementMode {
        get { return _isPlacementMode; }
        set { _isPlacementMode = value; }
    }

    public GameObject HamsterWheelPrefab
    {
        set { _hamsterWheelPrefab = value; }
    }

    public Product Product { 
        get { return _product; } 
        set { _product = value; } 
    }

    private void Start()
    {
        _dragCurrentPosition.action.performed += OnDrag;
        _dragCurrentPosition.action.canceled += (c) => _isDragging = false;
        _dragDelta.action.performed += OnDragDelta;
        _tapStartPosition.action.started += OnTap;
        _pressHoldAction.action.performed += OnLongTap;

        _center = new Vector2(_xrCamera.pixelWidth / 2, _xrCamera.pixelHeight / 2);
        
        _placementPrefab = Instantiate(_placementPrefab);
        _placementPrefab.SetActive(false);
        _rotationPrefab = Instantiate(_rotationPrefab);
        _rotationPrefab.SetActive(false);

        Transform cancleTr = _buttonUIPanel.transform.Find("CancelButton");
        Transform applyTr = _buttonUIPanel.transform.Find("ApplyButton");
        if (cancleTr != null)
        {
            cancleTr.GetComponent<Button>().onClick.AddListener(() =>
            {
                CancelAction.Invoke();
                _isPlacementMode = false;
                _isRotationMode = false;
                Destroy(_currentHamsterWheel.gameObject);
                _currentHamsterWheel = null;
            });
        }
        if (applyTr != null)
        {
           applyTr.GetComponent<Button>().onClick.AddListener(() => {
               ApplyAction.Invoke(Product);
               SavePlacementData();
               _isPlacementMode = false;
               _isRotationMode = false;
           });
        }
    }

    private void Update()
    {
        _placementPrefab.SetActive(_isPlacementMode && !_isRotationMode);
        _rotationPrefab.SetActive(_isPlacementMode && _isRotationMode);
        _buttonUIPanel.SetActive(_isPlacementMode);

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
        RaycastHit hitInfo;

        if (_isPlacementMode && !_isRotationMode && Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _targetLayer))
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

    private void OnDragDelta(InputAction.CallbackContext context)
    {
        Vector2 dragDeltaValue = context.ReadValue<Vector2>();
        //Debug.Log($"dragDelta => {dragDelta}");

        if (_isPlacementMode && _isRotationMode)
        {
            _currentHamsterWheel.transform.Rotate(new Vector3(0, -dragDeltaValue.x, 0));
            
        }

        if (dragDeltaValue.x >= 1 || dragDeltaValue.y >= 1) // 쳇바퀴 드래그 이동 중에 롱 탭 이벤트 발생하여 회전모드로 변경되는 현상 방지
        {
            _isDragging = true;
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
                //if (_hits[0].trackable is ARPlane plane)
                //{
                //    ARAnchor anchor = _arAnchorManager.AttachAnchor(plane, _hits[0].pose);
                    
                //}
                Debug.Log("쳇바퀴 생성");
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
                    Debug.Log("쳇바퀴 찾음");
                    _isPlacementMode = true;
                }
            }
        }
    }

    private void OnLongTap(InputAction.CallbackContext context)
    {
        if (!_isPlacementMode) return;
        Debug.Log($"OnLongTap => {context.time} isDragging: {_isDragging}");

        if (!_isDragging)
            _isRotationMode = !_isRotationMode;
    }

    private void SavePlacementData()
    {
        // 쳇바퀴 배치 정보 저장
        Placement placement = new Placement(_product.id, _currentHamsterWheel.transform.position, _currentHamsterWheel.transform.rotation);
        DataLoader dataLoader = new DataLoader();
        PlacementData placementData = dataLoader.Load<PlacementData>();
        placementData.placements.Add(placement);
        dataLoader.Save(placementData);
    }
}
