using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Travel : MonoBehaviour
{
    [SerializeField] private GameObject _travelIcon;
    [SerializeField] private TextMeshProUGUI _remainTravelText;
    [SerializeField] private SouvenirManager _souvenirManager;
    DateTime _startTime;
    DateTime _endTime;
    bool _isTraveling = false;
    Coroutine _HideCoroutine;
    private DataLoader _dataLoader;
    private TravelData _travelData;


    private void Awake()
    {
        _dataLoader = new DataLoader();
        _travelData = _dataLoader.Load<TravelData>();
    }

    private void Start()
    {
        if(_souvenirManager != null)
        {
            _souvenirManager.travelRefreshAction += RefreshTravel;
        }
        RefreshTravel();
    }

    /// <summary>
    /// 남은 시간을 새로고침하여 햄스터의 출발, 도착을 정하는 함수
    /// </summary>
    public void RefreshTravel()
    {
        _endTime = DateTime.ParseExact(_travelData.travelEndTime, "yyyy-MM-dd HH:mm:ss", null);
        if (_endTime < DateTime.Now)
        {
            _isTraveling = false;
            //도착
            //출발 시간 및 도착시간 재설정
            var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
            _travelData.travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            _travelData.travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");

            //재설정 된 시간 저장
            _dataLoader.Save(_travelData);

            //TODO :: 기념품 획득
            if(_souvenirManager != null)
            {
                _souvenirManager.CollectSouvenir();
            }
            //TODO :: 햄스터 보이도록 설정

            Debug.Log("도착");
        }

        _startTime = DateTime.ParseExact(_travelData.travelStartTime, "yyyy-MM-dd HH:mm:ss", null);
        if (_isTraveling == false && _startTime < DateTime.Now)
        {
            _isTraveling = true;
            //출발
            //TODO :: 햄스터 안보이도록 설정
        }

        if (_travelIcon != null)
        {
            _travelIcon.SetActive(_isTraveling);
        }
    }

    /// <summary>
    /// 여행 아이콘을 터치했을때 남은시간이 나오도록 하는 함수
    /// </summary>
    public void OnClickTravelIcon()
    {
        //TODO :: 시간이 표시 되는 중 여행이 끝나 아이콘이 사라지며 발생할 수 있는 오류 처리
        GameObject textObj = _remainTravelText.transform.parent.gameObject;
        //남은 시간 표시
        var remainTime = _endTime - DateTime.Now;
        _remainTravelText.text = $"{remainTime.Hours}h {remainTime.Minutes}m left";
        textObj.SetActive(true);
        //3초뒤 사라지게 하기

        if(_HideCoroutine != null)
        {
            StopCoroutine(_HideCoroutine);
        }
        _HideCoroutine = StartCoroutine(C_HideText(textObj));
    }

    private IEnumerator C_HideText(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.SetActive(false);
    }
}
