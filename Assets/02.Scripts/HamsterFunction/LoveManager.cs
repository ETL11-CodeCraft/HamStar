using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class LoveManager : MonoBehaviour
{
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _loveBtn; //애정주기 버튼
    [SerializeField] GameObject _lovePanel;  //애정주기 설명 버튼
    //[SerializeField] LayerMask _HamsterLayer;  
    private bool _isActiveLoveBtn = false;  // 버튼 활성화 확인
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private bool _isDragging = false;
    private Vector2 _startDragPosition;
    private float _minDragDistance = 20f;

    void Start()
    {
        _lovePanel.SetActive(false);

        _loveBtn.onClick.AddListener(GiveLove);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GiveLove()
    {
        _isActiveLoveBtn = !_isActiveLoveBtn;
        _lovePanel.SetActive(_isActiveLoveBtn);
        _dragCurrentPosition.action.started += OnDragStart;
        _dragCurrentPosition.action.performed += OnDraging;
        _dragCurrentPosition.action.canceled += OnDragEnd;

    }
    private void OnDragStart(InputAction.CallbackContext context)
    {
        Debug.Log("Drag Start !!!");
        Vector2 TouchPositoin = context.ReadValue<Vector2>();  //터치 포지션을 받아온다.
        
        Ray ray = _xrCamera.ScreenPointToRay(TouchPositoin);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "HamsterTouchRange")
            {
                _isDragging = true;
                _startDragPosition = TouchPositoin;
            }
        }
    }


    private void OnDraging(InputAction.CallbackContext context)
    {
        if (_isDragging)
        {
            Vector2 currentTouchPosition = context.ReadValue<Vector2>();
            Vector2 dragVector = currentTouchPosition - _startDragPosition;  //드래그 벡터 계산

            if (dragVector.magnitude > 20f)
            {
                Debug.Log("쓰다듬기 성공 !");
            }
        }
    }

    private void OnDragEnd(InputAction.CallbackContext context)
    {
        Debug.Log("DragEnd");
    }
}
