using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class Hamster : MonoBehaviour
{
    private float _detectionRange = 0.5f;
    private float _moveSpeed = 0.2f;
    [SerializeField] private GameObject _healingEffect;
    private Animator _animator;
    private BehaviorTree _behaviorTree;

    [SerializeField] private Travel _travelPrefab;
    private Travel _travelManager;

    [Header("Eat")]
    List<GameObject> _seeds = new List<GameObject>();
    private GameObject _currentSeed;
    private float _eatElapse = 0;
    private const float SEED_EATING_DURATION = 1f;
    [Header("MoveOrIdle")]
    private int _actFlag = -1;              // -1 : 이전 행동이 완료됨   0 : IDLE    1 : MOVE
    private float _idleElapse = 0;
    private const float IDLE_DURATION = 3f;
    private Vector3 _destination = Vector3.one;
    [Header("Poop")]
    [SerializeField] private GameObject _poopPrefab;
    [SerializeField] private InputActionReference _tapAction;
    private List<GameObject> _poopList = new List<GameObject>(4);
    public int poopCnt = 0;
    private int _maxPoopCnt = 4;

    private int _fullness = 100;
    private int _cleanliness = 100;
    private int _closeness = 100;
    private int _stress = 0;
    private bool _isDarken = false;

    [SerializeField] private GameObject _fullnessPrefab;
    [SerializeField] private GameObject _cleanlinessPrefab;
    [SerializeField] private GameObject _closenessPrefab;
    [SerializeField] private GameObject _stressPrefab;
    [SerializeField] private GameObject _darkenPrefab;
    private Slider _fullnessSlider;
    private Slider _cleanlinessSlider;
    private Slider _closenessSlider;
    private Slider _stressSlider;
    private Image _fullnessColor;
    private Image _cleanlinessColor;
    private Image _closenessColor;
    private Image _stressColor;
    private GameObject _darkenMark;
    private float _statRefreshInterval = 300;     //30분
    private Coroutine _increseStressCoroutine;

    private DataLoader _dataLoader;
    private HamsterStatData _hamsterStatData;

    public int fullness
    {
        get
        {
            return Mathf.Clamp(_fullness, 0, 100);
        }
        set
        {
            _fullness = Mathf.Clamp(value, 0, 100);
            _fullnessSlider.value = _fullness;

            if (_fullness >= 75)
            {
                _fullnessColor.color = Color.blue;
            }
            else if (_fullness >= 50)
            {
                _fullnessColor.color = Color.green;
            }
            else if (_fullness >= 25)
            {
                _fullnessColor.color = Color.yellow;
            }
            else
            {
                _fullnessColor.color = Color.red;
            }
        }
    }

    public int cleanliness
    {
        get
        {
            return Mathf.Clamp(_cleanliness, 0, 100);
        }
        set
        {
            _cleanliness = Mathf.Clamp(value, 0, 100);
            _cleanlinessSlider.value = _cleanliness;

            if (_cleanliness >= 75)
            {
                _cleanlinessColor.color = Color.blue;
            }
            else if (_cleanliness >= 50)
            {
                _cleanlinessColor.color = Color.green;
            }
            else if (_cleanliness >= 25)
            {
                _cleanlinessColor.color = Color.yellow;
            }
            else
            {
                _cleanlinessColor.color = Color.red;
            }
        }
    }

    public int closeness
    {
        get
        {
            return Mathf.Clamp(_closeness, 0, 100);
        }
        set
        {
            _closeness = Mathf.Clamp(value, 0, 100);
            _closenessSlider.value = _closeness;

            if (_closeness >= 75)
            {
                _closenessColor.color = Color.blue;
            }
            else if (_closeness >= 50)
            {
                _closenessColor.color = Color.green;
            }
            else if (_closeness >= 25)
            {
                _closenessColor.color = Color.yellow;
            }
            else
            {
                _closenessColor.color = Color.red;
            }
        }
    }

    public int stress
    {
        get
        {
            return Mathf.Clamp(_stress, 0, 100);
        }
        set
        {
            _stress = Mathf.Clamp(value, 0, 100);
            _stressSlider.value = _stress;

            if (_stress >= 75)
            {
                _stressColor.color = Color.red;
            }
            else if (_stress >= 50)
            {
                _stressColor.color = Color.yellow;
            }
            else if (_stress >= 25)
            {
                _stressColor.color = Color.green;
            }
            else
            {
                _stressColor.color = Color.blue;
            }
        }
    }

    public bool isDarken
    {
        get { return _isDarken; }
        set
        {
            _isDarken = value;

            if (_isDarken == false)
            {
                stress -= 100;
                if(_darkenMark)
                {
                    _darkenMark.SetActive(false);
                }
            }
            else
            {
                if(_darkenMark)
                {
                    _darkenMark.SetActive(true);
                }
            }
        }
    }

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _hamsterStatData = _dataLoader.Load<HamsterStatData>();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();

        //추후 수정예정
        var hamsterPanel = GameObject.Find("HamsterPanel");

        _travelManager = Instantiate(_travelPrefab, hamsterPanel.transform);
        _travelManager.hamster = this;

        _fullnessSlider = Instantiate(_fullnessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _cleanlinessSlider = Instantiate(_cleanlinessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _closenessSlider = Instantiate(_closenessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _stressSlider = Instantiate(_stressPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _darkenMark = Instantiate(_darkenPrefab, hamsterPanel.transform);
        _fullnessColor = _fullnessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _cleanlinessColor = _cleanlinessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _closenessColor = _closenessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _stressColor = _stressSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

        _fullnessSlider.maxValue = 100;
        _cleanlinessSlider.maxValue = 100;
        _closenessSlider.maxValue = 100;
        _stressSlider.maxValue = 100;

        var recentTime = DateTime.ParseExact(_hamsterStatData.recentChangedDate, "yyyy-MM-dd HH:mm:ss", null);
        var deltaTime = DateTime.Now - recentTime;

        fullness = _hamsterStatData.fullness - ((int)deltaTime.TotalMinutes / 30 * 2);
        cleanliness = _hamsterStatData.cleanliness - ((int)deltaTime.TotalMinutes / 30 * 2);
        closeness = _hamsterStatData.closeness - ((int)deltaTime.TotalMinutes / 30 * 2);
        var deltaStress = (4 - (int)deltaTime.TotalMinutes / 30 * 2) * 3;
        stress += _hamsterStatData.stress + deltaStress;

        if (stress >= 100)
        {
            if(_darkenMark)
            {
                _darkenMark.SetActive(true);
            }
        }
        else
        {
            if(_darkenMark)
            {
                _darkenMark.SetActive(false);
            }
        }

        _tapAction.action.performed += OnTap;

        _increseStressCoroutine = StartCoroutine(HamsterStatUpdate());
        StartCoroutine(DebugGeneratePoop());

        #region Behavior Tree
        //Behavior Tree
        _behaviorTree = new BehaviorTree();
        _behaviorTree
            .SetRoot(new SelectorNode())
                .Node(() =>
                {
                    if(stress >= 100)
                    {
                        isDarken = true;
                        return NodeState.Success;
                    }

                    isDarken = false;
                    return NodeState.Failure;
                })                  //IsDarken (흑화 상태에 들어가면 이후 행동을 진행하지 않음)
                .Sequence()
                    .Node(() =>
                    {
                        AssignNextSeed();

                        //만약 근처에 씨앗이 있다면
                        if (_currentSeed)
                        {
                            float distance = Vector3.Distance(gameObject.transform.position, _currentSeed.transform.position);

                            if (distance <= _detectionRange)
                            {
                                return NodeState.Success;
                            }
                        }

                        return NodeState.Failure;
                    })              //Is Seed On Place (탐지 가능한 범위 내에 씨앗이 있는지)
                    .Selector()
                        .Sequence()
                            .Node(() =>
                            {
                                if (_currentSeed)
                                {
                                    if (Vector3.Distance(transform.position, _currentSeed.transform.position) <= 0.1f)
                                    {
                                        return NodeState.Success;
                                    }
                                }
                                return NodeState.Failure;
                            })      //Is Seed Near Hamster (먹을 수 있는 범위 내에 씨앗이 있는지)
                            .Node(() =>
                            {
                                if (_currentSeed)
                                {
                                    _animator.SetBool("IsIdle", false);
                                    _animator.SetBool("isMove", false);
                                    _animator.SetBool("isEat", true);

                                    _eatElapse += Time.deltaTime;
                                    if (_eatElapse >= SEED_EATING_DURATION)
                                    {
                                        _seeds.Remove(_currentSeed);
                                        Destroy(_currentSeed);

                                        _currentSeed = null;
                                        _eatElapse = 0;

                                        return NodeState.Success;
                                    }
                                    else
                                    {
                                        return NodeState.Running;
                                    }
                                }

                                return NodeState.Failure;
                            })      //Eat (씨앗 먹기)
                        .CloseComposite()
                        .Node(() =>
                        {
                            if (_currentSeed)
                            {
                                Vector3 direction = (_currentSeed.transform.position - gameObject.transform.position).normalized;
                                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                                gameObject.transform.position += direction * _moveSpeed * Time.deltaTime;

                                _animator.SetBool("isIdle", false);
                                _animator.SetBool("isEat", false);
                                _animator.SetBool("isMove", true);

                                return NodeState.Success;
                            }

                            return NodeState.Failure;
                        })          //Move To Seed (씨앗 방향으로 이동하기)
                    .CloseComposite()
                .CloseComposite()
                .Selector()
                    .Node(() =>
                    {
                        //actFlag 가 0이면 IDLE, 1이면 MOVE
                        if (_actFlag == -1)
                        {
                            _actFlag = UnityEngine.Random.Range(0, 2);
                            _destination = new Vector3(UnityEngine.Random.Range(-100f, 100f), transform.position.y, UnityEngine.Random.Range(-100f, 100f));
                        }

                        return NodeState.Failure;
                    })              //Set Random Force Failure (actFlag를 설정하는 실패 노드)
                    .Sequence()
                        .Node(() =>
                        {
                            if (_actFlag == 0)
                            {
                                return NodeState.Success;
                            }

                            return NodeState.Failure;
                        })          //Is Random Value Idle (Idle 상태로 진입할지 확인하는 노드)
                        .Node(() =>
                        {
                            _animator.SetBool("isIdle", true);
                            _animator.SetBool("isMove", false);
                            _animator.SetBool("isEat", false);

                            _idleElapse += Time.deltaTime;
                            if (_idleElapse > IDLE_DURATION)
                            {
                                _idleElapse = 0f;
                                _actFlag = -1;

                                return NodeState.Success;
                            }

                            return NodeState.Running;
                        })          //Idle (가만히 있기)
                    .CloseComposite()
                    .Sequence()
                        .Node(() =>
                        {
                            if (_actFlag == 1)
                            {
                                return NodeState.Success;
                            }

                            return NodeState.Failure;
                        })          //Is Random Value Move (Move 상태로 진입할지 확인하는 노드)
                        .Node(() =>
                        {
                            _animator.SetBool("isIdle", false);
                            _animator.SetBool("isEat", false);
                            _animator.SetBool("isMove", true);

                            Vector3 direction = (_destination - gameObject.transform.position).normalized;
                            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                            gameObject.transform.position += direction * _moveSpeed * Time.deltaTime;

                            if ((_destination - gameObject.transform.position).magnitude < 0.1f)
                            {
                                _actFlag = -1;
                                return NodeState.Success;
                            }
                            if(!Physics.Raycast(transform.position + (transform.forward * 0.1f), Vector3.down,5f))
                            {
                                _actFlag = -1;
                                return NodeState.Success;
                            }

                            return NodeState.Running;
                        })          //Move (랜덤한 위치로 이동하기)
                    .CloseComposite()
                .CloseComposite()
            .CloseComposite();
        #endregion

        //poop init
        for(int i=0;i<_maxPoopCnt;i++)
        {
            var poop = Instantiate(_poopPrefab);
            poop.SetActive(false);
            _poopList.Add(poop);
        }

        
    }

    void Update()
    {
        _behaviorTree.Evaluate();
    }
    public void AddSeed(GameObject seed)
    {
        Debug.Log("Add Seed");
        _seeds.Add(seed);

        if (_currentSeed == null)
        {
            _currentSeed = seed;
        }
    }
    private void AssignNextSeed()
    {
        if (_seeds.Count > 0)
        {
            _currentSeed = _seeds[0];
        }
        else
        {
            _currentSeed = null;
        }
    }

    private void AddPoop()
    {
        foreach(var poop in  _poopList)
        {
            if(poop.gameObject.activeSelf == false)
            {
                poop.transform.position = transform.position;
                poop.SetActive(true);
                poopCnt++;
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Potion")
        {
            collision.gameObject.SetActive(false);
            GameObject healingEffect = Instantiate(_healingEffect, gameObject.transform.position, Quaternion.identity);


            //이펙트 생성 후 제거
            ParticleSystem particleSystem = _healingEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                Destroy(healingEffect, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(healingEffect, 3f);
            }
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        Debug.Log("TAP");
        Vector2 tapPosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(tapPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Poop"))
            {
                poopCnt--;
                hit.collider.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator HamsterStatUpdate()
    {
        while (true)
        {
            //임시 감소 수치
            fullness -= 2;
            cleanliness -= poopCnt * 1;
            closeness -= 2;

            var deltaStress = (4 - fullness / 25) + (4 - cleanliness / 25) + (4 - closeness / 25);
            stress += deltaStress;

            _hamsterStatData.fullness = fullness;
            _hamsterStatData.cleanliness = cleanliness;
            _hamsterStatData.closeness = closeness;
            _hamsterStatData.stress = stress;
            _hamsterStatData.recentChangedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _dataLoader.Save(_hamsterStatData);

            yield return new WaitForSeconds(_statRefreshInterval);
        }
    }

    IEnumerator DebugGeneratePoop()
    {
        while(true)
        {
            AddPoop();

            yield return new WaitForSeconds(5f);
        }
    }
}
