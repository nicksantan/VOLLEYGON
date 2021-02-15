using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChangeScript : MonoBehaviour
{
    public Slider VolumeSlider;
    public static float volume;
    public int whichType;
    public float baseMasterVolume = .55f;
    public AudioClip changeSound;

    private void Awake()
    {
        VolumeSlider = GetComponent<Slider>();

        // Set default value based on type
        float val;
        switch (whichType)
        {
            case 0:
                VolumeSlider.value = FileSystemLayer.Instance.masterVolume;
                break;
            case 1:
                VolumeSlider.value = FileSystemLayer.Instance.musicVolume; 
                break;
            case 2:
                VolumeSlider.value = FileSystemLayer.Instance.sfxVolume;
                break;
        }
    }

    void PlayChangedSound()
    {
        SoundManagerScript.instance.PlaySingle(changeSound);
    }

    void UpdateMusicVolume()
    {
        float savedMasterVolume = FileSystemLayer.Instance.masterVolume / 10f;
        float savedMusicVolume = FileSystemLayer.Instance.musicVolume / 10f;

        // Multiply master volume setting by default volume setting (.55);
        MusicManagerScript.Instance.masterVolume = baseMasterVolume * savedMasterVolume * savedMusicVolume;
		MusicManagerScript.Instance.StartRoot();
    }
  
    public void OnChangeMasterVolume(float value)
    {
        FileSystemLayer.Instance.masterVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("masterVolume", VolumeSlider.value);

        UpdateMusicVolume();
        PlayChangedSound();
    }

    public void OnChangeMusicVolume(float value)
    {
        FileSystemLayer.Instance.musicVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("musicVolume", VolumeSlider.value);
        
        UpdateMusicVolume();
        PlayChangedSound();
    }

    public void OnChangeSFXVolume(float value)
    {
        FileSystemLayer.Instance.sfxVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("sfxVolume", VolumeSlider.value);

        PlayChangedSound();
    }
}
