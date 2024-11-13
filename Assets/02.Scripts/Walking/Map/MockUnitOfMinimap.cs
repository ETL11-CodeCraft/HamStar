using UnityEngine;

public class MockUnitOfMinimap : IUnitOfMinimap
{
    public MockUnitOfMinimap()
    {
        gps = new GameObject(nameof(MockGPS)).AddComponent<MockGPS>();
    }

    public IGPS gps { get; }
}
