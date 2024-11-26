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
using System.Collections;

public class FeedingManager : MonoBehaviour
{
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] Camera _xrCamera;

    private LoveManager _loveManager;
    private PotionManager _potionManager;

    //Prefab
    [SerializeField] GameObject _sunflowerSeedPrefab;  //�عٶ�⾾ ������Ʈ 
    [SerializeField] GameObject _sunflowerGoldSeedPrefab;  //�عٶ�⾾ (Ư��) ������Ʈ 
    [SerializeField] GameObject _hamsterPrefab;  //�ܽ��� ������
    [SerializeField] LayerMask _draggable;

    //Button
    [SerializeField] Button _feedBtn; //���� ��ư 
    [SerializeField] Button _seedBtn; //���� ��ư
    [SerializeField] Button _goldSeedBtn; //��徾�� ��ư

    //Panel
    [SerializeField] GameObject _feedPanel; //���� ���� �г�
    [SerializeField] GameObject _feedPanel_darkModeWarning; //���� ���� �г�

    //EventTrigger
    [SerializeField] EventTrigger _seedBtnEventTrigger;
    [SerializeField] EventTrigger _goldSeedBtnTrigger;

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
        _feedPanel_darkModeWarning.SetActive(false);
        _feedPanel.SetActive(false);    // Feed Instruction panel 

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

                Vector3 spawnPosition = hitPose.position + new Vector3(0, 0.1f, 0); // Y������ 0.1m ����
                _spawnObject = Instantiate(_sunflowerSeedPrefab, spawnPosition, Quaternion.Euler(40f, -26f, 12f));
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

                if (hamsterScript.isDarken)
                {
                    StartCoroutine(ShowPanel(_feedPanel_darkModeWarning, 2f));
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

                Vector3 spawnPosition = hitPose.position + new Vector3(0, 0.1f, 0); // Y������ 0.1m ����
                _spawnObject = Instantiate(_sunflowerGoldSeedPrefab, spawnPosition, Quaternion.Euler(40f,-26f,12f));
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
                    _spawnObject.transform.position = hitPose.position + new Vector3(0, 0.15f, 0);
                    _spawnObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                    //_spawnObject.GetComponent<Rigidbody>().isKinematic = false;  //����ȿ�� ����
                }
            }
        });
        _seedBtnEventTrigger.triggers.Add(onPointerDragEntry);
        _goldSeedBtnTrigger.triggers.Add(onPointerDragEntry);

        //���� PointerUp �̺�Ʈ Ʈ����
        EventTrigger.Entry OnPointerUpEntry = new EventTrigger.Entry();
        OnPointerUpEntry.eventID = EventTriggerType.PointerUp;
        OnPointerUpEntry.callback.AddListener(eventData =>
        {
            if (_isDraging && _spawnObject != null)
            {
                _isDraging = false;
                Rigidbody rb = _spawnObject.GetComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.isKinematic = false;  //����ȿ�� ����

                rb.angularVelocity = Vector3.zero;

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

    }
    private void Start()
    {
        _loveManager = FindObjectOfType<LoveManager>();
        _potionManager = FindObjectOfType<PotionManager>();
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
        _loveManager?.SetFeedBtnInteractable(_isActiveFeed);
        _potionManager?.SetFeedBtnInteractable(_isActiveFeed);


        _isActiveFeed = !_isActiveFeed;
        _seedBtn.gameObject.SetActive(_isActiveFeed);
        _goldSeedBtn.gameObject.SetActive(_isActiveFeed);
        _feedPanel.SetActive(_isActiveFeed);
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

    public void SetFeedBtnInteractable(bool isInteractable)
    {
        if (_feedBtn != null)
        {
            _feedBtn.interactable = isInteractable;
        }
    }

    IEnumerator ShowPanel(GameObject gameObject, float seconds)
    {
        if (gameObject == null)
        {
            Debug.Log("gameObj is null");
            yield break;
        }
        gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
