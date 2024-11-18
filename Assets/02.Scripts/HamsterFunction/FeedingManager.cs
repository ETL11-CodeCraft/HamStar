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
    [SerializeField] InputActionReference _longPress;
    [SerializeField] GameObject _sunflowerSeedPrefab;  //�عٶ�⾾ ������Ʈ 
    [SerializeField] GameObject _sunflowerGoldSeedPrefab;  //�عٶ�⾾ (Ư��) ������Ʈ 
    [SerializeField] GameObject _hamsterPrefab;  //�ܽ��� ������
    [SerializeField] GameObject _potionPrefab;  //�ܽ��� ������
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _feedBtn; //���� ��ư 
    [SerializeField] Button _seedBtn; //���� ��ư
    [SerializeField] Button _goldSeedBtn; //��徾�� ��ư
    [SerializeField] Button _potionBtn; //���� ��ư
    [SerializeField] Button _loveBtn; //�����ֱ� ��ư

    bool _isActiveFeed = false;
    bool _isDraging = false;
    bool _isHamsterSpawned = false;
    float _clickTime; //Ŭ������ �ð�
    float _minClickTime = 1; //�ּ� Ŭ�� �ð� 
    GameObject _spawnObject;
    GameObject _selectedObject;
    GameObject _playerObject; 
    [SerializeField] LayerMask _draggable;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private void Awake()
    {
        _seedBtn.gameObject.SetActive(_isActiveFeed);
        _goldSeedBtn.gameObject.SetActive(_isActiveFeed);

        // ���� ��ư ������ �߰�
        if (_feedBtn != null)
        {
            _feedBtn.onClick.AddListener(OnShowOtherButton);
        }
        else
        {
            Debug.Log("_feedBtn is null");
        }

        // ���� ��ư ������ �߰� 
        if (_seedBtn != null)
        {
            _seedBtn.onClick.AddListener(OnMakeSeedButton);
            //EventTrigger trigger = _seedBtn.gameObject.AddComponent<EventTrigger>();

            ////Pointer Down �̺�Ʈ �߰�
            //EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            //pointerDownEntry.callback.AddListener((eventData) => { ButtonDown(); });
            //trigger.triggers.Add(pointerDownEntry);

            ////Pointer Up �̺�Ʈ �߰�
            //EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            //pointerUpEntry.callback.AddListener((eventData) => { ButtonUp(); });
            //trigger.triggers.Add(pointerUpEntry);
        }
        else
        {
            Debug.Log("Seed Button is not assigned.");
        }

        // Ư�� ��ư ������ �߰�
        if (_goldSeedBtn != null)
        {
            _goldSeedBtn.onClick.AddListener(OnMakeGoldSeedButton);
        }
        else
        {
            Debug.Log("goldSeedBtn is null");
        }

        if (_potionBtn != null)
        {
            _potionBtn.onClick.AddListener(OnMakePotion);
        }
        else
        {
            Debug.Log("potion is null");
        }
    }

    private void Start()
    {
        _longPress.action.performed += OnLongPress;
        _dragCurrentPosition.action.started += OnTouch;
        _dragCurrentPosition.action.performed += OnDrag;
        _dragCurrentPosition.action.canceled += OnTouchOut;

    }
    private void Update()
    {
        if (!_isHamsterSpawned)
        {
            Invoke("SpawnHamsterAtCenter", 5f);
        }

    }

    private void OnLongPress(InputAction.CallbackContext context)
    {
        _spawnObject = Instantiate(_sunflowerSeedPrefab, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
        _spawnObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);

        if (_playerObject != null)
        {
            Hamster hamsterScript = _playerObject.GetComponent<Hamster>();
            if (hamsterScript != null)
            {
                //seed ������ ������, Seeds����Ʈ�� �߰� 
                hamsterScript.AddSeed(_spawnObject);
            }
        }
    }

    private void OnTouch(InputAction.CallbackContext context)
    {

        Vector2 TouchPosition = context.ReadValue<Vector2>();
        RaycastHit hit;
        Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, _draggable))
        {
            _selectedObject = hit.collider.gameObject;
            _selectedObject.GetComponent<Rigidbody>().isKinematic = true;
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
        Debug.Log("OnDrag");
        if (_isDraging && _selectedObject != null)
        {
            // ��ġ �������� �����´�
            Vector2 touchPosition = context.ReadValue<Vector2>();

            Ray ray = _xrCamera.ScreenPointToRay(touchPosition);

            // AR ����ĳ��Ʈ ����
            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                // ù ��° ��Ʈ�� Ʈ���ͺ�(AR ���)�� ���� ������Ʈ ��ġ ����
                Pose hitPose = _hits[0].pose;
                _selectedObject.transform.position = hitPose.position - ray.direction * 0.05f;
            }
        }
    }

    private void OnTouchOut(InputAction.CallbackContext context)
    {
        if (_isDraging)
        {
            _isDraging = false;

            _selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            _selectedObject = null;
        }

    }
    private void OnShowOtherButton()
    {
        _isActiveFeed = !_isActiveFeed;
        _seedBtn.gameObject.SetActive(_isActiveFeed);
        _goldSeedBtn.gameObject.SetActive(_isActiveFeed);
    }
    private void OnMakePotion()
    {
        Debug.Log("Make Potion");
        _spawnObject = Instantiate(_potionPrefab, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
        _spawnObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);
    }
    private void OnMakeSeedButton()
    {
        _spawnObject = Instantiate(_sunflowerSeedPrefab, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
        _spawnObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);

        if (_playerObject != null) 
        {
            Hamster hamsterScript = _playerObject.GetComponent<Hamster>();
            if (hamsterScript != null)
            {
                //seed ������ ������, Seeds����Ʈ�� �߰� 
                hamsterScript.AddSeed(_spawnObject);
            }
        }
    }

    private void OnMakeGoldSeedButton()
    {
        _spawnObject = Instantiate(_sunflowerGoldSeedPrefab, _xrCamera.transform.position + _xrCamera.transform.forward * 0.5f, _xrCamera.transform.rotation);
        _spawnObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        _spawnObject.GetComponent<Rigidbody>().AddForce(_xrCamera.transform.forward * 90f, ForceMode.Force);

        if (_playerObject != null)
        {
            Hamster hamsterScript = _playerObject.GetComponent<Hamster>();
            if (hamsterScript != null)
            {
                //seed ������ ������, Seeds����Ʈ�� �߰� 
                hamsterScript.AddSeed(_spawnObject);
            }
        }
    }

    private void SpawnHamsterAtCenter()
    {
        // ȭ�� �߾� ��ġ�� �������� ����ĳ��Ʈ ����
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_arRaycastManager.Raycast(screenCenter, _hits, TrackableType.Planes))
        {
            if (!_isHamsterSpawned)
            {
                Pose hitPose = _hits[0].pose;  // ù ��°�� ������ ����� ��ġ ��������

                // �ܽ��� ������Ʈ ����
                _playerObject = Instantiate(_hamsterPrefab, hitPose.position, hitPose.rotation);

                _isHamsterSpawned = true;
                Debug.Log("�ܽ��Ͱ� ȭ�� �߾� �ٴڿ� �����Ǿ����ϴ�.");
            }
        }
        else
        {
            Debug.Log("AR ����� �������� �ʾҽ��ϴ�. �ܽ��͸� ������ �� �����ϴ�.");
        }
    }
}
