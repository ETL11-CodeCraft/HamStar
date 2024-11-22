using System;

/// <summary>
/// 기기의 위도, 경도 정보
/// </summary>
public interface IGPS
{
    /// <summary>
    /// 위도
    /// </summary>
    float latitude { get; }
    /// <summary>
    /// 경도
    /// </summary>
    float longitude { get; }
    /// <summary>
    /// map을 불러온 위도
    /// </summary>
    float latitudeMap { get; }
    /// <summary>
    /// map을 불러온 경도
    /// </summary>
    float longitudeMap { get; }

    event Action<float, float> onLocationChanged;
    event Action<float, float, float, float> onMapMoved;
}
