using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class Souvenir : MonoBehaviour
{
    [SerializeField] private Sprite _lockedImage;
    private Sprite _unlockedImage;
    public string SouvenirName { get; private set; }
    public Sprite SouvenirSprite { get; private set; }
    public string SouvenirDescription { get; private set; }
    public int SouvenirID { get; private set; }
    private bool _isCollected = false;
    public bool IsCollected 
    {
        get
        {
            return _isCollected;
        }
        set
        {
            if(value == true)
            {
                GetComponent<Image>().sprite = _unlockedImage;
                _souvenirButton.onClick.AddListener(() => { onClickAction?.Invoke(this); });
            }
        }
    }

    Button _souvenirButton;
    Image _souvenirImage;
    public Action<Souvenir> onClickAction;


    private void Awake()
    {
        _souvenirButton = GetComponent<Button>();
        _souvenirImage = GetComponent<Image>();
        GetComponent<Image>().sprite = _lockedImage;
    }
    private void Start()
    {
        _souvenirButton.onClick.AddListener(SoundManager.instance.PlayButtonSound);
    }
    public void SetSouvenir(string name, Sprite sprite, string desc, int id)
    {
        SouvenirName = name;
        SouvenirSprite = sprite;
        _unlockedImage = SouvenirSprite;
        SouvenirDescription = desc;
        SouvenirID = id;
    }
}
