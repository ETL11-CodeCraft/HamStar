using System;
using UnityEngine;

/// <summary>
/// ������ ���� ���·� �����ϰ�, ������ �浵�� ���� �Ÿ� ���
/// </summary>
public class DistanceCalculator
{
    private const double EarthRadius = 6378137f;    //������ ��� ������(���� m)
    private const double GoogleMapConst = 156543.03392f;    //���� �� �̹������� �Ÿ��� �ȼ��� �ٲ� �� ����� ���

    /// <summary>
    /// �� ��ǥ(����, �浵)�� �ѷ� ���̸� ���ϴ� �Լ�
    /// </summary>
    /// <param name="latitude1"></param>
    /// <param name="longitude1"></param>
    /// <param name="latitude2"></param>
    /// <param name="longitude2"></param>
    /// <returns></returns>
    public static double GetDistance(float latitude1, float longitude1, float latitude2, float longitude2)
    {
        double dLatitude = DegreeToRadian(latitude2 - latitude1);
        double dLongitude = DegreeToRadian(longitude2 - longitude1);

        double RadianLatitude1 = DegreeToRadian(latitude1);
        double RadianLatitude2 = DegreeToRadian(latitude2);

        //Haversine formula
        double a = Math.Pow(Math.Sin(dLatitude / 2), 2) + Math.Cos(RadianLatitude1) * Math.Cos(RadianLatitude2) * Math.Pow(Math.Sin(dLongitude / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = EarthRadius * c;

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
    public static double MeterPerPixelwithZoom(float zoom, float latitude)
    {
        double meterPerPixel = GoogleMapConst * Math.Cos(DegreeToRadian(latitude)) / Math.Pow(2, zoom);

        return meterPerPixel;
    }
}
