using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;

    [Header("Options")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private Resolution defaultResolution;
    private bool defaultFullscreen;
    private int defaultQuality;

    private Resolution[] resolutions;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        //Default
        defaultResolution = Screen.currentResolution;
        defaultFullscreen = Screen.fullScreen;
        defaultQuality = QualitySettings.GetQualityLevel();

        SetupResolutionOptions();
        SetupQualityOptions();
        fullscreenToggle.isOn = Screen.fullScreen;

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }
    private void SetupResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    private void SetupQualityOptions()
    {
        qualityDropdown.ClearOptions();
        List<string> options = new List<string>();

        string[] qualityNames = QualitySettings.names;
        for (int i = 0; i < qualityNames.Length; i++)
        {
            options.Add(qualityNames[i]);
        }

        qualityDropdown.AddOptions(options);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }
    public void OnResolutionChanged(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void OnQualityChanged(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
    public void ResetToDefault()
    {
        Screen.SetResolution(defaultResolution.width, defaultResolution.height, defaultFullscreen);
        QualitySettings.SetQualityLevel(defaultQuality);
    }
}