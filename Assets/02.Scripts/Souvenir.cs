using System;
using UnityEngine;
using UnityEngine.UI;

public class Souvenir : MonoBehaviour
{
    public string souvenirName { get; private set; }
    public Sprite souvenirSprite { get; private set; }
    public string souvenirDescription { get; private set; }
    public int souvenirID { get; private set; }


    public Button souvenirButton;
    public Action<Souvenir> onClickAction;


    private void Awake()
    {
        souvenirButton = GetComponent<Button>();
        souvenirButton.onClick.AddListener(() => { onClickAction?.Invoke(this); });
    }
    public void SetSouvenir(string name, Sprite sprite, string desc, int id)
    {
        souvenirName = name;
        souvenirSprite = sprite;
        GetComponent<Image>().sprite = souvenirSprite;
        souvenirID = id;
        souvenirDescription = desc;
    }
}
