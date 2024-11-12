using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;
using System;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class FeedingManager : MonoBehaviour
{
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] GameObject _sunflowerSeed;  //해바라기씨 오브젝트 
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _seedBtn; //먹이 버튼 
    bool _isDraging = false;
    GameObject _spawnObject;
    GameObject _selectedObject;
    [SerializeField] LayerMask _draggable;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private void Awake()
    {
        
        // 버튼 리스너 추가
        if (_seedBtn != null)
        {
            _seedBtn.onClick.AddListener(OnMakeSeedButton);
        }
        else
        {
            Debug.Log("Seed Button is not assigned.");
        }
    }

    private void Start()
    {
        _dragCurrentPosition.action.started += OnTouch;
        _dragCurrentPosition.action.performed += OnDrag;
        _dragCurrentPosition.action.canceled += TouchOut;
        
    }


    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 TouchPosition = context.ReadValue<Vector2>();
        RaycastHit hit;
        Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, _draggable))
        {
            _selectedObject = hit.collider.gameObject;
            Debug.Log("Object selected");
            _isDraging = true;
        }
        else
        {
            Debug.Log("선택된 오브젝트가 없습니다.");
        }
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (_isDraging && _selectedObject != null)
        {
            // 터치 또는 마우스의 현재 위치를 가져옵니다.
            Vector2 touchPosition = context.ReadValue<Vector2>();

            Ray ray = _xrCamera.ScreenPointToRay(touchPosition);

            // AR 레이캐스트 수행
            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                // 첫 번째 히트된 트랙터블(AR 평면)에 대해 오브젝트 위치 갱신
                Pose hitPose = _hits[0].pose;
                _selectedObject.transform.position = hitPose.position;
            }
        }
    }

    private void TouchOut(InputAction.CallbackContext context) 
    {
        if (_isDraging)
        {
            _isDraging = false;

            //_selectedObject.transform.position += Vector3.up;

            _selectedObject = null;
        }

    }

    private void OnMakeSeedButton()
    {
        _spawnObject = Instantiate(_sunflowerSeed, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
        _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);
    }
}
