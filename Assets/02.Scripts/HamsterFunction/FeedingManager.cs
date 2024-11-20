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
    [SerializeField] Camera _xrCamera;

    //Prefab
    [SerializeField] GameObject _sunflowerSeedPrefab;  //�عٶ�⾾ ������Ʈ 
    [SerializeField] GameObject _sunflowerGoldSeedPrefab;  //�عٶ�⾾ (Ư��) ������Ʈ 
    [SerializeField] GameObject _hamsterPrefab;  //�ܽ��� ������
    [SerializeField] GameObject _potionPrefab;  //�ܽ��� ������
    [SerializeField] LayerMask _draggable;

    //Button
    [SerializeField] Button _feedBtn; //���� ��ư 
    [SerializeField] Button _seedBtn; //���� ��ư
    [SerializeField] Button _goldSeedBtn; //��徾�� ��ư
    [SerializeField] Button _potionBtn; //���� ��ư
    [SerializeField] Button _loveBtn; //�����ֱ� ��ư

    //EventTrigger
    [SerializeField] EventTrigger _seedBtnEventTrigger;
    [SerializeField] EventTrigger _goldSeedBtnTrigger;
    [SerializeField] EventTrigger _potionBtnTrigger;


    //Image
    private RectTransform _rectTransform;
    [SerializeField] Image _potionImg;

    bool _isActiveFeed = false;
    bool _isDraging = false;
    bool _isHamsterSpawned = false;
    bool _isPressSeedBtn = false;
    bool _isPressGoldSeedBtn = false;
    GameObject _spawnObject;
    GameObject _selectedObject;
    GameObject _playerObject; 
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();


    private void Awake()
    {
        _potionImg.gameObject.SetActive(false);
        //�Ϲ� ���� PointerDown �̺�ƮƮ����
        EventTrigger.Entry onPointerDownEntry_seed = new EventTrigger.Entry();
        onPointerDownEntry_seed.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry_seed.callback.AddListener(eventData =>
        {
            Debug.Log("OnTouch");
            _isDraging = true;

            Vector2 TouchPosition = ((PointerEventData)eventData).position;
            Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);


            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                Pose hitPose = _hits[0].pose;

                _spawnObject = Instantiate(_sunflowerSeedPrefab, hitPose.position, Quaternion.identity);
                _spawnObject.GetComponent<Rigidbody>().isKinematic = true;  //�ʱ⿡�� �������� �ʵ���
            }
            else
            {
                Debug.Log("No AR Plane found for seed placement");
            }

            if (_playerObject != null)
            {
                Hamster hamsterScript = _playerObject.GetComponent<Hamster>();
                if (hamsterScript != null)
                {
                    //seed ������ ������, Seeds����Ʈ�� �߰� 
                    hamsterScript.AddSeed(_spawnObject);
                    Debug.Log("Seed����Ʈ�� �߰� seed count :");
                }
            }
        });
        _seedBtnEventTrigger.triggers.Add(onPointerDownEntry_seed);


        //Ư�� ���� PointerDown �̺�Ʈ Ʈ����
        EventTrigger.Entry onPointerDownEntry_goldseed = new EventTrigger.Entry();
        onPointerDownEntry_goldseed.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry_goldseed.callback.AddListener(eventData =>
        {
            Debug.Log("OnTouch");
            _isDraging = true;

            Vector2 TouchPosition = ((PointerEventData)eventData).position;
            Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);


            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                Pose hitPose = _hits[0].pose;

                _spawnObject = Instantiate(_sunflowerGoldSeedPrefab, hitPose.position, Quaternion.identity);
                _spawnObject.GetComponent<Rigidbody>().isKinematic = true;  //�ʱ⿡�� �������� �ʵ���
            }
            else
            {
                Debug.Log("No AR Plane found for seed placement");
            }

            if (_playerObject != null)
            {
                Hamster hamsterScript = _playerObject.GetComponent<Hamster>();
                if (hamsterScript != null)
                {
                    //seed ������ ������, Seeds����Ʈ�� �߰� 
                    hamsterScript.AddSeed(_spawnObject);
                    Debug.Log("Seed����Ʈ�� �߰� seed count :");
                }
            }
        });
        _goldSeedBtnTrigger.triggers.Add(onPointerDownEntry_goldseed);


        // Potion PointerDown �̺�Ʈ Ʈ���� 
        _rectTransform = _potionImg.GetComponent<RectTransform>();

        EventTrigger.Entry onPointerDownEntry_Potion = new EventTrigger.Entry();
        onPointerDownEntry_Potion.eventID = EventTriggerType.PointerDown;
        onPointerDownEntry_Potion.callback.AddListener(eventData => 
        {
            Vector2 TouchPosition = ((PointerEventData)eventData).position;

            _potionImg.gameObject.SetActive(true);
            _rectTransform.position = TouchPosition;
        });
        _potionBtnTrigger.triggers.Add(onPointerDownEntry_Potion);

        //Potion Drag �̺�Ʈ Ʈ���� 
        EventTrigger.Entry onPointerDragEntry_potion = new EventTrigger.Entry();
        onPointerDragEntry_potion.eventID = EventTriggerType.Drag;
        onPointerDragEntry_potion.callback.AddListener(eventData =>
        {
            Debug.Log("OnDrag");

            // ��ġ �������� �����´�
            Vector2 touchPosition = ((PointerEventData)eventData).position;
            _rectTransform.position = touchPosition;
        });

        _potionBtnTrigger.triggers.Add(onPointerDragEntry_potion);

        //Potion - PointerUp �̺�Ʈ Ʈ����
        EventTrigger.Entry OnPointerUpEntry_potion = new EventTrigger.Entry();
        OnPointerUpEntry_potion.eventID = EventTriggerType.PointerUp;
        OnPointerUpEntry_potion.callback.AddListener(eventData =>
        {
            _potionImg.gameObject.SetActive(false);

            Vector2 TouchPosition = ((PointerEventData)eventData).position;
            Ray ray = _xrCamera.ScreenPointToRay(TouchPosition);

            if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
            {
                Pose hitPose = _hits[0].pose;

                Vector3 spawnPosition = hitPose.position + new Vector3(0, 0.1f, 0);
                _spawnObject = Instantiate(_potionPrefab, spawnPosition, Quaternion.Euler(-58f,-131f,-34f));
                _spawnObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                Debug.Log("No AR Plane found for seed placement");
            }

            
        });
        _potionBtnTrigger.triggers.Add(OnPointerUpEntry_potion);

        //Drag �̺�Ʈ Ʈ���� 
        EventTrigger.Entry onPointerDragEntry = new EventTrigger.Entry();
        onPointerDragEntry.eventID = EventTriggerType.Drag;
        onPointerDragEntry.callback.AddListener(eventData =>
        {
            Debug.Log("OnDrag");
            if (_isDraging && _spawnObject != null)
            {
                // ��ġ �������� �����´�
                Vector2 touchPosition = ((PointerEventData)eventData).position;

                Ray ray = _xrCamera.ScreenPointToRay(touchPosition);

                // AR ����ĳ��Ʈ ����
                if (_arRaycastManager.Raycast(ray, _hits, TrackableType.Planes))
                {
                    // ù ��° ��Ʈ�� Ʈ���ͺ�(AR ���)�� ���� ������Ʈ ��ġ ����
                    Pose hitPose = _hits[0].pose;
                    _spawnObject.transform.position = hitPose.position + new Vector3(0, 0.1f, 0);
                    _spawnObject.GetComponent<Rigidbody>().isKinematic = false;  //����ȿ�� ����
                }
            }
        });
        _seedBtnEventTrigger.triggers.Add(onPointerDragEntry);
        _goldSeedBtnTrigger.triggers.Add(onPointerDragEntry);


        //PointerUp �̺�Ʈ Ʈ����
        EventTrigger.Entry OnPointerUpEntry = new EventTrigger.Entry();
        OnPointerUpEntry.eventID = EventTriggerType.PointerUp;
        OnPointerUpEntry.callback.AddListener(eventData =>
        {
            if (_isDraging && _spawnObject != null)
            {
                _isDraging = false;

                _spawnObject.GetComponent<Rigidbody>().isKinematic = false;  //����ȿ�� ����

                _spawnObject = null;  //���� ���� �ʱ�ȭ
                _isPressSeedBtn = false;
            }
        });
        _seedBtnEventTrigger.triggers.Add(OnPointerUpEntry);
        _goldSeedBtnTrigger.triggers.Add(OnPointerUpEntry);


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

        
        if (_potionBtn != null)
        {
            _potionBtn.onClick.AddListener(OnMakePotion);
        }
        else
        {
            Debug.Log("potion is null");
        }
    }
   
    private void Update()
    {
        if (!_isHamsterSpawned)
        {
            Invoke("SpawnHamsterAtCenter", 5f);
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
