using UnityEngine;

public class Seed : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }
}
