using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MixerVolumeSetter : MonoBehaviour
{    
    public AudioMixer mixer;
    public void SetMasterVolume(float sliderValue)
    {        
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20f);
    }
    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20f);
    }
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20f);
    }
}
