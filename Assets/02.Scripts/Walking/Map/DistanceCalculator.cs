using UnityEngine;

/// <summary>
/// ������ ���� ���·� �����ϰ�, ������ �浵�� ���� �Ÿ� ���
/// </summary>
public class DistanceCalculator
{
    private const float EarthRadius = 6378137f;    //������ ��� ������(���� m)
    private const float GoogleMapConst = 156543.03392f;    //���� �� �̹������� �Ÿ��� �ȼ��� �ٲ� �� ����� ���

    /// <summary>
    /// �� ��ǥ(����, �浵)�� �ѷ� ���̸� ���ϴ� �Լ�
    /// </summary>
    /// <param name="latitude1"></param>
    /// <param name="longitude1"></param>
    /// <param name="latitude2"></param>
    /// <param name="longitude2"></param>
    /// <returns></returns>
    public static float GetDistance(float latitude1, float longitude1, float latitude2, float longitude2)
    {
        float dLatitude = DegreeToRadian(latitude2 - latitude1);
        float dLongitude = DegreeToRadian(longitude2 - longitude1);

        float RadianLatitude1 = DegreeToRadian(latitude1);
        float RadianLatitude2 = DegreeToRadian(latitude2);

        //Haversine formula
        float a = Mathf.Pow(Mathf.Sin(dLatitude / 2f), 2) + Mathf.Cos(RadianLatitude1) * Mathf.Cos(RadianLatitude2) * Mathf.Pow(Mathf.Sin(dLongitude / 2f), 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float distance = EarthRadius * c;

        Debug.Log($"({latitude1},{longitude1}),({latitude2},{longitude2}) Distance : { distance}");
        Debug.Log($"MeterPerPixel : {MeterPerPixelwithZoom(17, latitude1)}");

        return distance;
    }

    /// <summary>
    /// ������ �浵�� pi ������ �ٲٴ� �Լ�
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    private static float DegreeToRadian(float degree)
    {
        return degree * (Mathf.PI / 180f);
    }

    /// <summary>
    /// �� �ȼ��� �Ÿ�(m)�� ���ϴ� �Լ�
    /// </summary>
    /// <param name="zoom"></param>
    /// <param name="latitude"></param>
    /// <returns></returns>
    public static float MeterPerPixelwithZoom(float zoom, float latitude)
    {
        float meterPerPixel = GoogleMapConst * Mathf.Cos(DegreeToRadian(latitude)) / Mathf.Pow(2, zoom);

        return meterPerPixel;
    }
}
