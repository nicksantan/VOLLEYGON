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

    private void Awake()
    {
        VolumeSlider = GetComponent<Slider>();

        // Set default value based on type
        float val;
        switch (whichType)
        {
            case 0:
                VolumeSlider.value = FileSystemLayer.Instance.masterVolume;
              //  MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f); 
                break;
            case 1:
                VolumeSlider.value = FileSystemLayer.Instance.musicVolume; 
           //     MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f);
                break;
            case 2:
                VolumeSlider.value = FileSystemLayer.Instance.sfxVolume;
                break;
        }

    }
  
    public void OnChangeMasterVolume(float value)
    {
        FileSystemLayer.Instance.masterVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("masterVolume", VolumeSlider.value);
        // Multiply master volume setting by default volume setting (.55);
        MusicManagerScript.Instance.masterVolume = baseMasterVolume * (FileSystemLayer.Instance.musicVolume / 10f) * (VolumeSlider.value / 10f);
    }

    public void OnChangeMusicVolume(float value)
    {
        FileSystemLayer.Instance.musicVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("musicVolume", VolumeSlider.value);
        
        MusicManagerScript.Instance.masterVolume = baseMasterVolume * (FileSystemLayer.Instance.masterVolume / 10f) * (VolumeSlider.value / 10f);
    }

    public void OnChangeSFXVolume(float value)
    {
        FileSystemLayer.Instance.sfxVolume = VolumeSlider.value;
        FileSystemLayer.Instance.SaveFloatPref("sfxVolume", VolumeSlider.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
