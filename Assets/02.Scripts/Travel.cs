using System;
using UnityEngine;
using UnityEngine.XR;

public class Travel : MonoBehaviour
{
    string travelStartTime;
    string travelEndTime;
    bool isTraveling = false;

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

    public void RefreshTravel()
    {
        DateTime endTime = DateTime.ParseExact(travelEndTime, "yyyy-MM-dd HH:mm:ss", null);
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

            Debug.Log("도착");
        }

        DateTime startTime = DateTime.ParseExact(travelStartTime, "yyyy-MM-dd HH:mm:ss", null);
        if (isTraveling == false && startTime < DateTime.Now)
        {
            isTraveling = true;
            //출발
            //TODO :: 햄스터 안보이도록 설정
            //TODO :: 여행중 마크 생성

        }
    }
}
