using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _souvenirName;
    [SerializeField] private TextMeshProUGUI _souvenirDesc;
    [SerializeField] private Image _souvenirImg;
    [SerializeField] private RectTransform _contents;
    private RectTransform _descRect;

    private void Awake()
    {
        _descRect = _souvenirDesc.GetComponent<RectTransform>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_descRect.rect.height != _contents.rect.height)
        {
            _contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _descRect.rect.height);
        }
    }

    public void ActiveInfo(Souvenir souvenir)
    {
        _souvenirName.text = souvenir.SouvenirName;
        _souvenirDesc.text = souvenir.SouvenirDescription;
        _souvenirImg.sprite = souvenir.SouvenirSprite;

        _contents.localPosition = new Vector3(_contents.localPosition.x, 0);

        gameObject.SetActive(true);

        
    }

    public void DeactiveInfo()
    {
        gameObject.SetActive(false);
    }
}
