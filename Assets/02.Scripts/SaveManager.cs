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

    #endregion

    #region Hamster
    public static HamsterStatData hamsterStatData { get; private set; }
    private const string HAMSTER_DATA_PATH = "/HamsterData.json";


    public static HamsterStatData LoadHamsterData()
    {
        if (File.Exists(Application.dataPath + HAMSTER_DATA_PATH))
        {
            string loadData = File.ReadAllText(Application.dataPath + HAMSTER_DATA_PATH);
            hamsterStatData = JsonUtility.FromJson<HamsterStatData>(loadData);
            Debug.Log(loadData);
        }
        else
        {
            //세이브데이터 초기화
            //게임을 처음 실행한다면 해당 코드 실행
            hamsterStatData = new HamsterStatData();

            hamsterStatData.fullness = 100;
            hamsterStatData.cleanliness = 100;
            hamsterStatData.closeness = 100;
            hamsterStatData.stress = 0;

            SaveHamsterData();
        }

        return hamsterStatData;
    }

    public static void SaveHamsterData()
    {
        File.WriteAllText(Application.dataPath + HAMSTER_DATA_PATH, JsonUtility.ToJson(hamsterStatData));
    }

    public static void SetHamsterData(int fullness, int cleanliness, int closeness, int stress)
    {
        hamsterStatData.fullness = fullness;
        hamsterStatData.cleanliness = cleanliness;
        hamsterStatData.closeness = closeness;
        hamsterStatData.stress = stress;

        SaveHamsterData();
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

public class HamsterStatData
{
    public int fullness;
    public int cleanliness;
    public int closeness;
    public int stress;
}
