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
    [SerializeField] Button _loveBtn; //�����ֱ� ��ư
    [SerializeField] GameObject _lovePanel;  //�����ֱ� ���� ��ư
    //[SerializeField] LayerMask _HamsterLayer;  
    private bool _isActiveLoveBtn = false;  // ��ư Ȱ��ȭ Ȯ��
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
        Vector2 TouchPositoin = context.ReadValue<Vector2>();  //��ġ �������� �޾ƿ´�.
        
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
            Vector2 dragVector = currentTouchPosition - _startDragPosition;  //�巡�� ���� ���

            if (dragVector.magnitude > 20f)
            {
                Debug.Log("���ٵ�� ���� !");
            }
        }
    }

    private void OnDragEnd(InputAction.CallbackContext context)
    {
        Debug.Log("DragEnd");
    }
}
