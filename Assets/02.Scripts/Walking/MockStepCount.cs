using UnityEngine;

/// <summary>
/// ������ ���� ��, ������ �Ϻ� ���� ��, �������� ��
/// </summary>
public class MockStepCount : MonoBehaviour, IStepCount
{
    public int stepCount { get; } = 10;

    public int[] thisWeekStepCount { get; } = { 1, 2, 3, 4, 5, 6, 7 };

    public int chestCount { get; } = 1;

    public int firstStepCount { get; } = 60000;
}