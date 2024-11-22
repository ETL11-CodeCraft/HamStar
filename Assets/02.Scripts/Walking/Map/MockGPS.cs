using System;
using UnityEngine;

/// <summary>
/// �׽�Ʈ�� ������ ���� �� �浵
/// </summary>
public class MockGPS : MonoBehaviour, IGPS
{
    public float latitude { get; } = 37.5139002f;

    public float longitude { get; } = 127.0294921f;

    public float latitudeMap { get; } = 37.5139012f;

    public float longitudeMap { get; } = 127.0294931f;

    public event Action<float, float> onLocationChanged;
    public event Action<float, float, float, float> onMapMoved;
}
