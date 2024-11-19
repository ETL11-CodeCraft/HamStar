using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    public Action TriggerEnterAction;
    public Action TriggerExitAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HamsterWheel") || other.gameObject.CompareTag("Poop") || other.gameObject.CompareTag("feed"))
        {
            TriggerEnterAction?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HamsterWheel") || other.gameObject.CompareTag("Poop") || other.gameObject.CompareTag("feed"))
        {
            TriggerExitAction?.Invoke();
        }
    }
}
