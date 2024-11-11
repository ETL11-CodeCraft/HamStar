using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Travel : MonoBehaviour
{
    [SerializeField] private GameObject travelIcon;
    [SerializeField] private TextMeshProUGUI remainTravelText;
    string travelStartTime;
    DateTime startTime;
    string travelEndTime;
    DateTime endTime;
    bool isTraveling = false;
    Coroutine HideCoroutine;

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
            travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");
        }

        RefreshTravel();
    }

    /// <summary>
    /// 남은 시간을 새로고침하여 햄스터의 출발, 도착을 정하는 함수
    /// </summary>
    public void RefreshTravel()
    {
        endTime = DateTime.ParseExact(travelEndTime, "yyyy-MM-dd HH:mm:ss", null);
        if (endTime < DateTime.Now)
        {
            isTraveling = false;
            //도착
            //출발 시간 및 도착시간 재설정
            var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
            travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");

            //TODO :: 기념품 획득
            //TODO :: 햄스터 보이도록 설정
            //TODO :: 여행중 마크 제거
            if(travelIcon != null)
            {
                travelIcon.SetActive(false);
            }

            Debug.Log("도착");
        }

        startTime = DateTime.ParseExact(travelStartTime, "yyyy-MM-dd HH:mm:ss", null);
        if (isTraveling == false && startTime < DateTime.Now)
        {
            isTraveling = true;
            //출발
            //TODO :: 햄스터 안보이도록 설정
            //TODO :: 여행중 마크 생성
            if(travelIcon != null)
            {
                travelIcon.SetActive(true);
            }
        }
    }

    public void OnClickTravelIcon()
    {
        GameObject textObj = remainTravelText.transform.parent.gameObject;
        //TODO :: 남은 시간 표시
        var remainTime = endTime - DateTime.Now;
        remainTravelText.text = $"{remainTime.Hours}h {remainTime.Minutes}m left";
        textObj.SetActive(true);
        //TODO :: 3초뒤 사라지게 하기

        if(HideCoroutine != null)
        {
            StopCoroutine(HideCoroutine);
        }
        HideCoroutine = StartCoroutine(C_HideText(textObj));
    }

    private IEnumerator C_HideText(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        obj.SetActive(false);
    }
}
