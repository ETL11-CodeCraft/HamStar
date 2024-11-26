using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Slider _BGMSlider;
    [SerializeField] private Slider _SFXSlider;

    private void OnEnable()
    {
        _BGMSlider.value = SoundManager.instance.bgmSource.volume;
        _SFXSlider.value = SoundManager.instance.sfxSource.volume;

        _BGMSlider.onValueChanged.AddListener(OnChangeBGMSlider);
        _SFXSlider.onValueChanged.AddListener(OnChangeSFXSlider);
    }

    public void OnChangeBGMSlider(float value)
    {
        SoundManager.instance.SetBGMVolume(value);
    }

    public void OnChangeSFXSlider(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
