using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Travel : MonoBehaviour
{
    [SerializeField] private GameObject _travelIcon;
    [SerializeField] private TextMeshProUGUI _remainTravelText;
    string _travelStartTime;
    DateTime _startTime;
    string _travelEndTime;
    DateTime _endTime;
    bool _isTraveling = false;
    Coroutine _HideCoroutine;


    private void Start()
    {
        if (false)
        {
            //데이터를 로드하는 부분
            //현재는 SaveData를 받아올 수 없음
        }
        else
        {
            var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
            _travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            _travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");
        }

        RefreshTravel();
    }

    /// <summary>
    /// 남은 시간을 새로고침하여 햄스터의 출발, 도착을 정하는 함수
    /// </summary>
    public void RefreshTravel()
    {
        _endTime = DateTime.ParseExact(_travelEndTime, "yyyy-MM-dd HH:mm:ss", null);
        if (_endTime < DateTime.Now)
        {
            _isTraveling = false;
            //도착
            //출발 시간 및 도착시간 재설정
            var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
            _travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            _travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");

            //TODO :: 기념품 획득
            //TODO :: 햄스터 보이도록 설정

            Debug.Log("도착");
        }

        _startTime = DateTime.ParseExact(_travelStartTime, "yyyy-MM-dd HH:mm:ss", null);
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

    public void OnClickTravelIcon()
    {
        GameObject textObj = _remainTravelText.transform.parent.gameObject;
        //TODO :: 남은 시간 표시
        var remainTime = _endTime - DateTime.Now;
        _remainTravelText.text = $"{remainTime.Hours}h {remainTime.Minutes}m left";
        textObj.SetActive(true);
        //TODO :: 3초뒤 사라지게 하기

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
