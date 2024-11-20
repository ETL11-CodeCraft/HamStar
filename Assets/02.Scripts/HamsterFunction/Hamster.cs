using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Hamster : MonoBehaviour
{
    public enum HamsterState
    {
        Idle, Move, Eat
    }

    private float _detectionRange = 0.5f;
    private float _moveSpeed = 0.2f;
    [SerializeField] private HamsterState _currentState;
    [SerializeField] private GameObject _healingEffect;
    private Animator _animator;
    List<GameObject> _seeds = new List<GameObject>();
    private GameObject _currentSeed;

    private int _fullness = 100;
    private int _cleanliness = 100;
    private int _closeness = 100;
    private int _stress = 0;

    [SerializeField] private GameObject _fullnessPrefab;
    [SerializeField] private GameObject _cleanlinessPrefab;
    [SerializeField] private GameObject _closenessPrefab;
    [SerializeField] private GameObject _stressPrefab;
    private Slider _fullnessSlider;
    private Slider _cleanlinessSlider;
    private Slider _closenessSlider;
    private Slider _stressSlider;
    private Image _fullnessColor;
    private Image _cleanlinessColor;
    private Image _closenessColor;
    private Image _stressColor;
    private float _stressInterval = 5f;
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

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _hamsterStatData = _dataLoader.Load<HamsterStatData>();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentState = HamsterState.Idle;

        //추후 수정예정
        var hamsterPanel = GameObject.Find("HamsterPanel");

        _fullnessSlider = Instantiate(_fullnessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _cleanlinessSlider = Instantiate(_cleanlinessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _closenessSlider = Instantiate(_closenessPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _stressSlider = Instantiate(_stressPrefab, hamsterPanel.transform).GetComponent<Slider>();
        _fullnessColor = _fullnessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _cleanlinessColor = _cleanlinessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _closenessColor = _closenessSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _stressColor = _stressSlider.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

        _fullnessSlider.maxValue = 100;
        _cleanlinessSlider.maxValue = 100;
        _closenessSlider.maxValue = 100;
        _stressSlider.maxValue = 100;

        fullness = _hamsterStatData.fullness;
        cleanliness = _hamsterStatData.cleanliness;
        closeness = _hamsterStatData.closeness;
        stress = _hamsterStatData.stress;

        _increseStressCoroutine = StartCoroutine(IncreseStress());
    }

    void Update()
    {
        switch (_currentState)
        {
            case HamsterState.Idle:
                Idle();
                break;
            case HamsterState.Move:
                Move();
                break;
            case HamsterState.Eat:
                Eat();
                break;
        }

        if (_currentSeed != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, _currentSeed.transform.position);

            if (distance <= _detectionRange && _currentState != HamsterState.Eat)
            {
                _currentState = HamsterState.Move;
            }
        }
        else   //currentSeed�� null�̸� 
        {
            _currentState = HamsterState.Idle;
            AssignNextSeed();
        }
    }
    public void AddSeed(GameObject seed)
    {
        Debug.Log("Add Seed");
        _seeds.Add(seed);  //����Ʈ��  seed �߰� 

        //ù ��° ���� Ÿ����
        if (_currentSeed == null)
        {
            _currentSeed = seed;
        }
    }
    private void AssignNextSeed()
    {
        if (_seeds.Count > 0)
        {
            Debug.Log("���� ������ �Ҵ��մϴ�.");
            _currentSeed = _seeds[0];
            _currentState = HamsterState.Move;
        }
        else
        {
            Debug.Log("���� ������ �����ϴ�.");
            _currentSeed = null;
            _currentState = HamsterState.Idle;
        }
    }

    void Idle()
    {
        Debug.Log("Hamster Idle");
        _animator.SetBool("isMove", false);
        _animator.SetBool("isEat", false);
        _animator.SetBool("isIdle", true);
    }

    void Move()
    {
        if (_currentSeed != null)
        {
            Vector3 direction = (_currentSeed.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            gameObject.transform.position += direction * _moveSpeed * Time.deltaTime;
            Debug.Log("Hamster Move");
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isMove", true);

            if (Vector3.Distance(transform.position, _currentSeed.transform.position) <= 0.1f)
            {
                _currentState = HamsterState.Eat;
            }
        }
        else
        {
            _currentState = HamsterState.Idle;
            AssignNextSeed();
            return;
        }

    }

    void Eat()
    {
        Debug.Log("Hamster Eat");
        _animator.SetBool("isMove", false);
        _animator.SetBool("isEat", true);

        if (_currentSeed != null)
        {
            Destroy(_currentSeed);
            _seeds.RemoveAt(0);
            Debug.Log("SeedCount:"+_seeds.Count);
            _currentSeed = null;
        }

        Invoke("StopEat", 1f);
    }

    void StopEat()
    {
        Debug.Log("Hamster stoped eating");
        _animator.SetBool("isEat", false);
        _currentState = HamsterState.Idle;

        AssignNextSeed();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Potion")
        {
            Debug.Log("Hamster�� Potion�� ��ҽ��ϴ�.");
            collision.gameObject.SetActive(false);
            Instantiate(_healingEffect, gameObject.transform.position, Quaternion.identity);
        }
    }
    #region DEBUG
    public void DEBUG_fullnessUp()
    {
        fullness += 10;
    }
    public void DEBUG_fullnessDown()
    {
        fullness -= 10;
    }
    #endregion
    IEnumerator IncreseStress()
    {
        while (true)
        {
            var deltaStress = (4 - fullness / 25) + (4 - cleanliness / 25) + (4 - closeness / 25);
            stress += deltaStress;
            _dataLoader.Save(_hamsterStatData);

            yield return new WaitForSeconds(_stressInterval);
        }
    }
}
