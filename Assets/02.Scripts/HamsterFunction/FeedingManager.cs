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
    [SerializeField] GameObject _sunflowerSeedPrefab;  //해바라기씨 오브젝트 
    [SerializeField] GameObject _sunflowerGoldSeedPrefab;  //해바라기씨 (특식) 오브젝트 
    [SerializeField] GameObject _hamsterPrefab;  //햄스터 프리팹
    [SerializeField] GameObject _potionPrefab;  //햄스터 프리팹
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _feedBtn; //먹이 버튼 
    [SerializeField] Button _seedBtn; //씨앗 버튼
    [SerializeField] Button _goldSeedBtn; //골드씨앗 버튼
    [SerializeField] Button _potionBtn; //물약 버튼
    [SerializeField] Button _loveBtn; //애정주기 버튼

    bool _isActiveFeed = false;
    bool _isDraging = false;
    bool _isHamsterSpawned = false;
    float _clickTime; //클릭중인 시간
    float _minClickTime = 1; //최소 클릭 시간 
    GameObject _spawnObject;
    GameObject _selectedObject;
    GameObject _playerObject; 
    [SerializeField] LayerMask _draggable;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private void Awake()
    {
        _seedBtn.gameObject.SetActive(_isActiveFeed);
        _goldSeedBtn.gameObject.SetActive(_isActiveFeed);

        // 먹이 버튼 리스너 추가
        if (_feedBtn != null)
        {
            _feedBtn.onClick.AddListener(OnShowOtherButton);
        }
        else
        {
            Debug.Log("_feedBtn is null");
        }

        // 씨앗 버튼 리스너 추가 
        if (_seedBtn != null)
        {
            _seedBtn.onClick.AddListener(OnMakeSeedButton);
            //EventTrigger trigger = _seedBtn.gameObject.AddComponent<EventTrigger>();

            ////Pointer Down 이벤트 추가
            //EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            //pointerDownEntry.callback.AddListener((eventData) => { ButtonDown(); });
            //trigger.triggers.Add(pointerDownEntry);

            ////Pointer Up 이벤트 추가
            //EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            //pointerUpEntry.callback.AddListener((eventData) => { ButtonUp(); });
            //trigger.triggers.Add(pointerUpEntry);
        }
        else
        {
            Debug.Log("Seed Button is not assigned.");
        }

        // 특식 버튼 리스너 추가
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
                //seed 프리팹 생성시, Seeds리스트에 추가 
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
            Debug.Log("선택된 오브젝트가 없습니다.");
        }
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        Debug.Log("OnDrag");
        if (_isDraging && _selectedObject != null)
        {
            // 터치 포지션을 가져온다
            Vector2 touchPosition = context.ReadValue<Vector2>();

            Ray ray = _xrCamera.ScreenPointToRay(touchPosition);

            // AR 레이캐스트 수행
            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                // 첫 번째 히트된 트랙터블(AR 평면)에 대해 오브젝트 위치 갱신
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
                //seed 프리팹 생성시, Seeds리스트에 추가 
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
                //seed 프리팹 생성시, Seeds리스트에 추가 
                hamsterScript.AddSeed(_spawnObject);
            }
        }
    }

    private void SpawnHamsterAtCenter()
    {
        // 화면 중앙 위치를 기준으로 레이캐스트 수행
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_arRaycastManager.Raycast(screenCenter, _hits, TrackableType.Planes))
        {
            if (!_isHamsterSpawned)
            {
                Pose hitPose = _hits[0].pose;  // 첫 번째로 감지된 평면의 위치 가져오기

                // 햄스터 오브젝트 생성
                _playerObject = Instantiate(_hamsterPrefab, hitPose.position, hitPose.rotation);

                _isHamsterSpawned = true;
                Debug.Log("햄스터가 화면 중앙 바닥에 생성되었습니다.");
            }
        }
        else
        {
            Debug.Log("AR 평면이 감지되지 않았습니다. 햄스터를 생성할 수 없습니다.");
        }
    }
}
