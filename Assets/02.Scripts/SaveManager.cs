using NUnit.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

    #region Inventory
    public static InventoryData inventoryData { get; private set; }
    private const string INVENTORY_DATA_PATH = "/InventoryData.json";

    public static InventoryData LoadInventoryData()
    {
        if (File.Exists(Application.dataPath + INVENTORY_DATA_PATH))
        {
            string input = File.ReadAllText(Application.dataPath + INVENTORY_DATA_PATH);
            inventoryData = JsonConvert.DeserializeObject<InventoryData>(input);
            Debug.Log($"{INVENTORY_DATA_PATH} reac text: \n{input}");
            Debug.Log($"inventory.coin => {inventoryData.coin}");
        }
        else
        {
            inventoryData = new InventoryData();
            inventoryData.quantityForProductId = new Dictionary<int, int>(0);
            SaveInventoryData();
        }

        return inventoryData;
    }

    public static void SaveInventoryData()
    {
        string output = JsonConvert.SerializeObject(inventoryData);
        File.WriteAllText(Application.dataPath + INVENTORY_DATA_PATH, output);
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
    #region Souvenir

    public static SouvenirData souvenirData { get; private set; }
    private const string SOUVENIR_DATA_PATH = "/SouvenirData.json";


    public static SouvenirData LoadSouvenirData()
    {
        if (File.Exists(Application.dataPath + SOUVENIR_DATA_PATH))
        {
            string loadData = File.ReadAllText(Application.dataPath + SOUVENIR_DATA_PATH);
            souvenirData = JsonUtility.FromJson<SouvenirData>(loadData);
            Debug.Log(loadData);
        }
        else
        {
            //세이브데이터 초기화
            //게임을 처음 실행한다면 해당 코드 실행
            souvenirData = new SouvenirData();

            SaveSouvenirData();
        }

        return souvenirData;
    }

    /// <summary>
    /// SaveData의 내용을 기반으로 Json 파일 생성
    /// </summary>
    public static void SaveSouvenirData()
    {
        File.WriteAllText(Application.dataPath + SOUVENIR_DATA_PATH, JsonUtility.ToJson(souvenirData));
    }

    public static void AddCollectedSouvenir(int id)
    {
        souvenirData.collectedSouvenir.Add(id);

        SaveSouvenirData();
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

/// <summary>
/// 걸음 수 관련 데이터를 관리하는 클래스
/// </summary>
[System.Serializable]
public class WalkData
{
    public string pastWalkCount;   //어제까지의 걸음 수
    public string[] thisWeekWalkCount;  //금주의 일간 걸음 수
    public string chestCount;   //보물상자의 수
    public string currentPlayDate;  //최근 플레이 날짜
}
public class SouvenirData
{
    public List<int> collectedSouvenir = new List<int>(8);
}
/// <summary>
/// 보유 아이템 데이터를 관리하는 클래스
/// </summary>
[System.Serializable]
public class InventoryData
{
    public int coin;
    public Dictionary<int, int> quantityForProductId;
}

public class HamsterStatData
{
    public int fullness;
    public int cleanliness;
    public int closeness;
    public int stress;
}
