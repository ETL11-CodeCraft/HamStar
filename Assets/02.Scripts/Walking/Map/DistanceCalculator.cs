using UnityEngine;

/// <summary>
/// ������ ���� ���·� �����ϰ�, ������ �浵�� ���� �Ÿ� ���
/// </summary>
public class DistanceCalculator
{
    private const float EarthRadius = 6371009f;    //������ ��� ������(���� m)

    public static float GetDistance(float latitude1, float longitude1, float latitude2, float longitude2)
    {
        float dLatitude = DegreeToRadian(latitude2 - latitude1);
        float dLongitude = DegreeToRadian(longitude2 - longitude1);

        float RadianLatitude1 = DegreeToRadian(latitude1);
        float RadianLatitude2 = DegreeToRadian(latitude2);

        //Haversine formula
        float a = Mathf.Pow(Mathf.Sin(dLatitude / 2), 2) + Mathf.Cos(RadianLatitude1) * Mathf.Cos(RadianLatitude2) * Mathf.Pow(Mathf.Sin(dLongitude / 2), 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float distance = EarthRadius * c;

        return distance;
    }

    private static float DegreeToRadian(float degree)
    {
        return degree * (Mathf.PI / 180);
    }
}
