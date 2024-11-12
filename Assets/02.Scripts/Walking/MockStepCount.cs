using UnityEngine;

/// <summary>
/// 임의의 걸음 수, 금주의 일별 걸음 수, 보물상자 수
/// </summary>
public class MockStepCount : MonoBehaviour, IStepCount
{
    public int stepCount { get; } = 10;

    public int[] thisWeekStepCount { get; } = { 1, 2, 3, 4, 5, 6, 7 };

    public int chestCount { get; } = 1;

    public int firstStepCount { get; } = 60000;
}