using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Text;
using System.Collections;

public class LoveManager : MonoBehaviour
{
    [SerializeField] InputActionReference _dragCurrentPosition;
    [SerializeField] Camera _xrCamera;
    [SerializeField] Button _loveBtn; //애정주기 버튼
    [SerializeField] GameObject _lovePanel_Instruction;  //패널1_ 햄스터를 쓰다듬어주세요
    [SerializeField] GameObject _lovePanel2_LovePlus30;  //패널2_쓰다듬기 슬라이드가 다 찼을 때 (+30)
    [SerializeField] GameObject _lovePanel3_LoveFull;  //패널3_애정도가 이미 100일 때
    [SerializeField] GameObject _lovePanel4_darkenWarning;  //패널3_애정도가 이미 100일 때
    private bool _isActiveLoveBtn = false;  // 버튼 활성화 확인
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private bool _isDragging = false;
    private Vector2 _startDragPosition;
    private float _minDragDistance = 20f;

    private Coroutine _fullClosenessCoroutine;
    private FeedingManager _feedingManager;
    private PotionManager _potionManager;

    //파티클시스템 
    [SerializeField] private ParticleSystem _heart;
    private ParticleSystem _heartInstance;

    //슬라이더
    [SerializeField] Slider _loveSlider; //슬라이더 추가
    private float _loveIncreaseAmount = 1f; //증가할 애정도의 양 
    private float _maxLoveValue = 100;


    void Start()
    {
        //하트 파티클 생성 & stop
        if (_heart != null)
        {
            _heartInstance = Instantiate(_heart,transform);
            _heartInstance.Stop();
        }
        _feedingManager = FindObjectOfType<FeedingManager>();
        _potionManager = FindObjectOfType<PotionManager>();
        _loveSlider.gameObject.SetActive(false);
        _loveSlider.interactable = false;
        _lovePanel_Instruction.SetActive(false);
        _lovePanel2_LovePlus30.SetActive(false);   
        _lovePanel3_LoveFull.SetActive(false);   
        _lovePanel4_darkenWarning.SetActive(false);

        _loveBtn.onClick.AddListener(GiveLove);
    }

    //파티클 위치를 햄스터 위치로 설정
    private void SetPariticlePosition(Vector3 hamsterPosition)
    {
        if (_heartInstance != null)
        {
            _heartInstance.transform.position = hamsterPosition;
        }
    }
    private void GiveLove()
    {
        _feedingManager?.SetFeedBtnInteractable(_isActiveLoveBtn);
        _potionManager?.SetFeedBtnInteractable(_isActiveLoveBtn);
        _loveSlider.value = 0;
        _isActiveLoveBtn = !_isActiveLoveBtn;
        _loveSlider.gameObject.SetActive(_isActiveLoveBtn);
        _lovePanel_Instruction.SetActive(_isActiveLoveBtn);
        _dragCurrentPosition.action.started += OnDragStart;
        _dragCurrentPosition.action.performed += OnDraging;
        _dragCurrentPosition.action.canceled += OnDragEnd;

        GameManager.instance.cantSwipe = !GameManager.instance.cantSwipe;

    }
    private void OnDragStart(InputAction.CallbackContext context)
    {
        Debug.Log("Drag Start !!!");
        Vector2 TouchPositoin = context.ReadValue<Vector2>();  //터치 포지션을 받아온다.

        Ray ray = _xrCamera.ScreenPointToRay(TouchPositoin);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "HamsterTouchRange")   //햄스터의 터치 범위(등)에 닿을 때만 
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

            Ray ray = _xrCamera.ScreenPointToRay(currentTouchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "HamsterTouchRange")
                {
                    if (dragVector.magnitude > 40f)
                    {
                        Debug.Log("쓰다듬기 성공 !");
                        var hamster = hit.collider.transform.parent.GetComponent<Hamster>();

                        IncreaseLoveValue(hamster);


                        SetPariticlePosition(hit.collider.transform.position);
                        //Particle System 재생 
                        if (!_heartInstance.isPlaying)
                        {
                            _heartInstance.Play();
                        }
                        
                    }
                }
            }
            
        }
    }

    private void OnDragEnd(InputAction.CallbackContext context)
    {
        Debug.Log("DragEnd");
        _isDragging = false;
        _heart.Stop();

        if (_heartInstance != null && _heartInstance.isPlaying)
        {
            _heartInstance.Stop();
        }
    }

    private void IncreaseLoveValue(Hamster hamster)
    {
        if(!hamster)
        {
            Debug.Log("햄스터 스크립트에 접근할 수 없음");
            return;
        }

        if (!hamster.isDarken)  //햄스터가 흑화상태가 아닐 때
        {
            if (hamster.closeness >= 100)
            {
                //애정도가 100이상일 경우 
                if (_fullClosenessCoroutine != null)
                {
                    StopCoroutine(_fullClosenessCoroutine);
                }

                _fullClosenessCoroutine = StartCoroutine(ShowPanel(_lovePanel3_LoveFull, 2f));
                return;
            }

            _loveSlider.value += _loveIncreaseAmount;

            if (_loveSlider.value >= _maxLoveValue)
            {
                _loveSlider.value = 0;
                Debug.Log("슬라이더 최대치 도달");
                hamster.closeness += 30;
                StartCoroutine(ShowPanel(_lovePanel2_LovePlus30, 2f));

            }
        }
        else   //햄스터가 흑화상태일 때
        {
            StartCoroutine(ShowPanel(_lovePanel4_darkenWarning, 2f));
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
            _loveSlider.value = 0;
        }

        _fullClosenessCoroutine = null;
    }

    public void SetFeedBtnInteractable(bool isInteractable)
    {
        if (_loveBtn != null)
        {
            _loveBtn.interactable = isInteractable;
        }
    }
}
