using System.Collections;
using UnityEngine;

public class PopupUI: MonoBehaviour
{
    private WaitForSeconds _popupDuration = new WaitForSeconds(1f); // 팝업 떠 있는 시간
    private WaitForSeconds _closeDelay = new WaitForSeconds(0.2f); // 닫히는 애니메이션 시간

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