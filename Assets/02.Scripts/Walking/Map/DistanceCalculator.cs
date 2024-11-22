using UnityEngine;

/// <summary>
/// 지구를 구의 형태로 가정하고, 위도와 경도에 따른 거리 계산
/// </summary>
public class DistanceCalculator
{
    private const float EarthRadius = 6378137f;    //지구의 평균 반지름(단위 m)
    private const float GoogleMapConst = 156543.03392f;    //구글 맵 이미지에서 거리를 픽셀로 바꿀 때 사용할 상수

    /// <summary>
    /// 두 좌표(위도, 경도)의 둘레 길이를 구하는 함수
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
    /// 위도나 경도를 pi 단위로 바꾸는 함수
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    private static float DegreeToRadian(float degree)
    {
        return degree * (Mathf.PI / 180f);
    }

    /// <summary>
    /// 한 픽셀당 거리(m)를 구하는 함수
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
