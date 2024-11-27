using System;
using UnityEngine;

public class Poop : MonoBehaviour
{
    public Action<GameObject> onFallObject;

    private void Update()
    {
        if(transform.position.y < -20)
        {
            onFallObject?.Invoke(gameObject);
        }
    }
}
