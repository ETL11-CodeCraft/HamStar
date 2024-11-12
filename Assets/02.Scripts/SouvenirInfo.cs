using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI souvenirName;
    [SerializeField] private TextMeshProUGUI souvenirDesc;
    [SerializeField] private Image souvenirImg;


    public void ActiveInfo(Souvenir souvenir)
    {
        souvenirName.text = souvenir.souvenirName;
        souvenirDesc.text = souvenir.souvenirDescription;
        souvenirImg.sprite = souvenir.souvenirSprite;

        gameObject.SetActive(true);
    }

    public void DeactiveInfo()
    {
        gameObject.SetActive(false);
    }
}
