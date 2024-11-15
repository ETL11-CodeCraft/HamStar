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

    event Action<float, float> onLocationChanged;
}
