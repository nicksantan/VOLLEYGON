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
                val = PlayerPrefs.GetFloat("masterVolume");
                VolumeSlider.value = PlayerPrefs.HasKey("masterVolume") ? val : 10f;
              //  MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f); 
                break;
            case 1:
                 val = PlayerPrefs.GetFloat("musicVolume");
                VolumeSlider.value = PlayerPrefs.HasKey("musicVolume") ? val : 10;
                float masterVolume = PlayerPrefs.HasKey("masterVolume") ? PlayerPrefs.GetFloat("masterVolume") : 10f;
           //     MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f);
                break;
            case 2:
                 val = PlayerPrefs.GetFloat("sfxVolume");
                VolumeSlider.value = PlayerPrefs.HasKey("sfxVolume") ? val : 10f;
                break;
        }

    }
  
    public void OnChangeMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("masterVolume", VolumeSlider.value);
        float test = PlayerPrefs.GetFloat("masterVolume");
        MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f);
        Debug.Log(test);
    }

    public void OnChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
        MusicManagerScript.Instance.masterVolume = baseMasterVolume * (VolumeSlider.value / 10f);
    }

    public void OnChangeSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("sfxVolume", VolumeSlider.value);
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
