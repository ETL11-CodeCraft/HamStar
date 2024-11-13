using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveManager
{
    #region Travel
    public static TravelData travelData { get; private set; }
    private const string TRAVEL_DATA_PATH = "/TravelData.json";


    public static TravelData LoadTravelData()
    {
        if (File.Exists(Application.dataPath + TRAVEL_DATA_PATH))
        {
            string loadData = File.ReadAllText(Application.dataPath + TRAVEL_DATA_PATH);
            travelData = JsonUtility.FromJson<TravelData>(loadData);
            Debug.Log(loadData);
        }
        else
        {
            //세이브데이터 초기화
            //게임을 처음 실행한다면 해당 코드 실행
            travelData = new TravelData();

            var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
            travelData.travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
            travelData.travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");

            SaveTravelData();
        }

        return travelData;
    }

    /// <summary>
    /// SaveData의 내용을 기반으로 Json 파일 생성
    /// </summary>
    public static void SaveTravelData()
    {
        File.WriteAllText(Application.dataPath + TRAVEL_DATA_PATH, JsonUtility.ToJson(travelData));
    }

    public static void SetTravelData(string travelStartTime, string travelEndTime)
    {
        travelData.travelStartTime = travelStartTime;
        travelData.travelEndTime = travelEndTime;

        SaveTravelData();
    }

    #endregion
}

/// <summary>
/// 여행 데이터를 관리하는 클래스
/// </summary>
[System.Serializable]
public class TravelData
{
    public string travelStartTime;  //여행을 시작할 시간
    public string travelEndTime;    //여행이 끝나고 돌아올 시간
}
