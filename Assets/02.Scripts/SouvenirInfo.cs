using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _souvenirName;
    [SerializeField] private TextMeshProUGUI _souvenirDesc;
    [SerializeField] private Image _souvenirImg;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActiveInfo(Souvenir souvenir)
    {
        _souvenirName.text = souvenir.souvenirName;
        _souvenirDesc.text = souvenir.souvenirDescription;
        _souvenirImg.sprite = souvenir.souvenirSprite;

        gameObject.SetActive(true);
    }

    public void DeactiveInfo()
    {
        gameObject.SetActive(false);
    }
}