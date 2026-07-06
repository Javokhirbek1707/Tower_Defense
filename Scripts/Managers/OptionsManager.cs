using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _volumeSlider;

    [Header("Brightness")]
    [SerializeField]
    private Image _blackOverlay;
    [SerializeField]
    private Slider _brightnessSlider;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        float brightness = PlayerPrefs.GetFloat("Brightness", 1f);

        _volumeSlider.value = volume;
        _brightnessSlider.value = brightness;

        ApplyVolume(volume);
        ApplyBrightness(brightness);

        _volumeSlider.onValueChanged.AddListener(ApplyVolume);
        _brightnessSlider.onValueChanged.AddListener(ApplyBrightness);
    }

    private void ApplyVolume(float value)
    {
        float db = value <= 0.001f ? -80f : Mathf.Log10(value) * 20f;
        _audioMixer.SetFloat("BG_Music", db);

        PlayerPrefs.SetFloat("Volume", value);
    }

    private void ApplyBrightness(float value)
    {
        Color c = _blackOverlay.color;
        c.a = 1f - value;
        _blackOverlay.color = c;

        PlayerPrefs.SetFloat("Brightness", value);
    }
}