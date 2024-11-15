using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hamster : MonoBehaviour
{
    private int _fullness = 100;
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

    private int _cleanliness = 100;
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

    private int _closeness = 100;
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

    private int _stress = 0;
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

    [SerializeField] private Slider _fullnessSlider;
    [SerializeField] private Slider _cleanlinessSlider;
    [SerializeField] private Slider _closenessSlider;
    [SerializeField] private Slider _stressSlider;
    [SerializeField] private Image _fullnessColor;
    [SerializeField] private Image _cleanlinessColor;
    [SerializeField] private Image _closenessColor;
    [SerializeField] private Image _stressColor;
    private float _stressInterval = 5f;
    private Coroutine _increseStressCoroutine;
    private DataLoader _dataLoader;
    private HamsterStatData _hamsterStatData;

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _hamsterStatData = _dataLoader.Load<HamsterStatData>();
    }

    private void Start()
    {
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
