using UnityEngine;

[CreateAssetMenu(fileName = "SouvenirSpec", menuName = "Scriptable Objects/SouvenirSpec")]
public class SouvenirSpec : ScriptableObject
{
    public string SouvenirName;
    public Sprite SouvenirSprite;
    public string SouvenirDescription;
    public int SouvenirID;
}
