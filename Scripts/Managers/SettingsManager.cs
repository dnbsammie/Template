using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    [Header("Managers")]
    public AudioManager audioManager;
    public UIManager uiManager;
    public InputManager inputManager;
    public VideoManager videoManager;
    [Header("Option Sections")]
    public GameObject audioSection;
    public GameObject controlSection;
    public GameObject generalSection;
    public GameObject prefSection;
    public GameObject videoSection;
    [Header("Items")]
    public Button resetButton;
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
    void Start()
    {
        ActivateSection(audioSection);
    }
    void Update()
    {
        
    }
    public void ActivateSection(GameObject newSection)
    {
        audioSection.SetActive(false);
        controlSection.SetActive(false);
        generalSection.SetActive(false);
        prefSection.SetActive(false);
        videoSection.SetActive(false);

        newSection.SetActive(true);
    }
    public void ResetAllSettings()
    {
        if (videoManager != null) videoManager.ResetToDefault();
        if (audioManager != null) audioManager.ResetToDefault();
    }
}