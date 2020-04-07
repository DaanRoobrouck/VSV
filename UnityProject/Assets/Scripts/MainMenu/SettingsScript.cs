using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Slider VolumeSlider;
    public Slider SensitivitySlider;

    public AudioSource audioSource;
    public FirstPersonAIO FP;

    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource.volume = VolumeSlider.value;
        FP.mouseSensitivity = SensitivitySlider.value * 3;
    }
}
