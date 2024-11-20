using UnityEngine.InputSystem.Android;

public interface IStepCount
{
    int stepCount { get; }

    //int[] thisWeekStepCount { get; }

    int coinChestCount { get; }

    int pastChestCount { get; }

    //int firstStepCount { get; }

    int GetChestCount();

    void ResetChestCount();
}