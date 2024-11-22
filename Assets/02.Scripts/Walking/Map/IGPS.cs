using System;

/// <summary>
/// ����� ����, �浵 ����
/// </summary>
public interface IGPS
{
    /// <summary>
    /// ����
    /// </summary>
    float latitude { get; }
    /// <summary>
    /// �浵
    /// </summary>
    float longitude { get; }
    /// <summary>
    /// map�� �ҷ��� ����
    /// </summary>
    float latitudeMap { get; }
    /// <summary>
    /// map�� �ҷ��� �浵
    /// </summary>
    float longitudeMap { get; }

    event Action<float, float> onLocationChanged;
    event Action<float, float, float, float> onMapMoved;
}
