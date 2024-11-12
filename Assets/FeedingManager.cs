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

public class SelectObject : MonoBehaviour
{
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] GameObject _sunflowerSeed;  //해바라기씨 오브젝트 
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _seedBtn; //먹이 버튼 
    int _seedCount = 10; //해바라기씨 개수
    bool _isDraging = false;
    GameObject _spawnObject;
    GameObject _selectedObject;
    [SerializeField] LayerMask _draggable;

    private void Awake()
    {
        
        // 버튼 리스너 추가
        if (_seedBtn != null)
        {
            _seedBtn.onClick.AddListener(OnMakeSeedButton);
        }
        else
        {
            Debug.LogError("Seed Button is not assigned.");
        }
    }

    private void Start()
    {
        _dragCurrentPosition.action.started += OnTouch;
        _dragCurrentPosition.action.performed += OnDrag;
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
        }
        else
        {
            Debug.Log("선택된 오브젝트가 없습니다.");
        }
    }

    //private void OnDrag(InputAction.CallbackContext context)
    //{
    //    Vector2 dragPosition = context.ReadValue<Vector2>();
    //    //스크린에 터치한 포지션 값을 월드 스페이스의 좌표로 바꾼다.
    //    Ray ray = _xrCamera.ScreenPointToRay(dragPosition);
    //    if (Physics.Raycast(ray, float.PositiveInfinity))
    //    {
            
    //    }
    //    Debug.Log(worldPosition);

    //    _spawnObject.transform.position = worldPosition;
    //}

    private void OnMakeSeedButton()
    {
        if (_seedCount <= 0)  //먹이 개수가 0 이상일 때만 버튼 누를 수 있음 
        {
            //버튼 비활성화 추가하기
            return;
        }
        else
        {
            _seedCount--;
            _spawnObject = Instantiate(_sunflowerSeed, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
            _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);

        }
    }
}
