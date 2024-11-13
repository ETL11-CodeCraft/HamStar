using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class Souvenir : MonoBehaviour
{
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
                _souvenirButton.onClick.AddListener(() => { onClickAction?.Invoke(this); });
                _souvenirImage.color = Color.white;
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
        _souvenirImage.color = Color.black;
    }
    public void SetSouvenir(string name, Sprite sprite, string desc, int id)
    {
        SouvenirName = name;
        SouvenirSprite = sprite;
        GetComponent<Image>().sprite = SouvenirSprite;
        SouvenirDescription = desc;
        SouvenirID = id;
    }
}
