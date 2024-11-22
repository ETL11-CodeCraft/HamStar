using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    [SerializeField] private LayerMask _arPlaneLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _arPlaneLayerMask) != 0)
        {
            Debug.Log("물약이 햄스터에 닿지 않아 제거됩니다");
            Destroy(gameObject);
            //물약 개수 변하지 않도록 코드 추가하기
        }
    }
}
