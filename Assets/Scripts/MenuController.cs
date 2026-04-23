using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class MenuController : MonoBehaviour
{
    public GameObject menu;

    public AudioMixer audioMixer;

    

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ShowMenu();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        resolutionDropdown.value = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ShowMenu()
    {
        SoundManager.Instance.PlayUI();
        menu.SetActive(!menu.activeSelf);
    }

    public void setVolume(float value)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(value) * 20);  
    }
}
