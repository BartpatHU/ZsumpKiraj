using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle FSToggle;

    Resolution[] resolutions;

    public float volumeSAVE;
    public int resolutionSAVE;
    public int graphicsSAVE;
    public bool fullscreenSAVE;




    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();


        int currentResolutionIndex = 0;
        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width  + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        LoadSettings();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionSAVE = resolutionIndex;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 40);
        volumeSAVE = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsSAVE = qualityIndex;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullscreenSAVE = isFullScreen;
    }

    public void LoadSettings()
    {
        GameData data = SaveSystem.LoadGame();
        volumeSlider.value = data.volume;
        resolutionDropdown.value = data.resolution;
        qualityDropdown.value = data.graphics;
        FSToggle.isOn = data.fullscreen;
    }

    public void SaveSettings()
    {
        SaveSystem.SaveGame(this);
    }

}
