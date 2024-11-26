using System;
using System.Collections.Generic;
using UnityEngine;


public class TravelData
{
    public string travelStartTime;  //여행을 시작할 시간
    public string travelEndTime;    //여행이 끝나고 돌아올 시간

    public TravelData()
    {
        var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
        travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
        travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");
    }
}

public class WalkData
{
    public int pastChestCount;   //오픈되지 않을 보물상자의 수

    public WalkData() 
    {
        pastChestCount = -1;
    }
}

public class SouvenirData
{
    public List<int> collectedSouvenir = new List<int>(8);

    public SouvenirData() { }
}

[System.Serializable]
public struct InventoryItem
{
    public int productId;
    public int quantity;
}

public class InventoryData
{
    public int coin = 300;
    public List<InventoryItem> quantityForProductId = new List<InventoryItem>();

    public InventoryData() { }
}

public class HamsterStatData
{
    public int fullness;
    public int cleanliness;
    public int closeness;
    public int stress;
    public string recentChangedDate;

    public HamsterStatData()
    {
        fullness = 100;
        cleanliness = 100;
        closeness = 100;
        stress = 0;
        recentChangedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

[Serializable]
public struct Placement
{
    public int productId;
    public Vector3 position;
    public Quaternion rotation;
    public float latitude;
    public float longitude;

    public Placement(int id,  Vector3 position, Quaternion rotation, float latitude, float longitude)
    {
        this.productId = id;
        this.position = position;
        this.rotation = rotation;
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

public class PlacementData
{
    public List<Placement> placements = new List<Placement>();

    public PlacementData() { }
}

public class StartDate
{
    public string startDate;

    public StartDate()
    {
        startDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}