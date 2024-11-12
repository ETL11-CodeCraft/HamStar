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
    [SerializeField] GameObject _sunflowerSeed;  //�عٶ�⾾ ������Ʈ 
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _seedBtn; //���� ��ư 
    bool _isDraging = false;
    GameObject _spawnObject;
    GameObject _selectedObject;
    [SerializeField] LayerMask _draggable;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private void Awake()
    {
        
        // ��ư ������ �߰�
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
            Debug.Log("���õ� ������Ʈ�� �����ϴ�.");
        }
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (_isDraging && _selectedObject != null)
        {
            // ��ġ �Ǵ� ���콺�� ���� ��ġ�� �����ɴϴ�.
            Vector2 touchPosition = context.ReadValue<Vector2>();

            Ray ray = _xrCamera.ScreenPointToRay(touchPosition);

            // AR ����ĳ��Ʈ ����
            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                // ù ��° ��Ʈ�� Ʈ���ͺ�(AR ���)�� ���� ������Ʈ ��ġ ����
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
