using System;
using System.Collections.Generic;
using UnityEngine;

public class TravelData
{
    public string travelStartTime;  //������ ������ �ð�
    public string travelEndTime;    //������ ������ ���ƿ� �ð�

    public TravelData()
    {
        var rand = UnityEngine.Random.Range(-3.5f, 3.5f);
        travelStartTime = DateTime.Now.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
        travelEndTime = DateTime.Now.AddHours(16 + rand).ToString("yyyy-MM-dd HH:mm:ss");
    }
}

public class WalkData
{
    public string pastWalkCount;   //���������� ���� ��
    public string[] thisWeekWalkCount;  //������ �ϰ� ���� ��
    public string chestCount;   //���������� ��
    public string currentPlayDate;  //�ֱ� �÷��� ��¥

    public WalkData() { }
}

public class SouvenirData
{
    public List<int> collectedSouvenir = new List<int>(8);

    public SouvenirData() { }
}

[System.Serializable]
public struct inventoryItem
{
    public int productId;
    public int quantity;
}
public class InventoryData
{
    public int coin;
    public List<inventoryItem> quantityForProductId = new List<inventoryItem>();

    public InventoryData() { }
}

public class HamsterStatData
{
    public int fullness;
    public int cleanliness;
    public int closeness;
    public int stress;

    public HamsterStatData()
    {
        fullness = 100;
        cleanliness = 100;
        closeness = 100;
        stress = 0;
    }
}