using System.Collections;
using UnityEngine;

public class PopupUI: MonoBehaviour
{
    private WaitForSeconds _popupDuration = new WaitForSeconds(1f); // �˾� �� �ִ� �ð�
    private WaitForSeconds _closeDelay = new WaitForSeconds(0.2f); // ������ �ִϸ��̼� �ð�

    private IEnumerator C_Popup()
    {
        yield return _popupDuration;
        gameObject.SetActive(false);
    }

    public void Popup()
    {
        gameObject.SetActive(true);
        StartCoroutine(C_Popup());
    }
}