using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
public enum PlayMode
{
    Sequential,
    Random
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Options")]
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider speechSlider;
    public Slider voiceSlider;
    public TMP_Dropdown playModeDropdown;
    public TMP_Dropdown speakerModeDropdown;
    public float minDb = -80f;
    public float maxDb = 20f;
    [Header("Player")]
    public AudioSource audioSource;
    public List<AudioClip> backgroundMusic;
    public PlayMode playMode = PlayMode.Sequential;
    private int currentSongIndex = -1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        speechSlider.onValueChanged.AddListener(SetSpeechVolume);
        voiceSlider.onValueChanged.AddListener(SetVoiceVolume);

        playModeDropdown.onValueChanged.AddListener(OnPlayModeChanged);
        InitializeDropdown();

        currentSongIndex = PlayerPrefs.GetInt("CurrentSongIndex", -1);

        PlayNextSong();
        InitializeSliders();
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }
    private void OnPlayModeChanged(int index)
    {
        playMode = (PlayMode)index;
        PlayNextSong();
    }
    void PlayNextSong()
    {
        if (backgroundMusic.Count == 0)
        {
            return;
        }
        if (playMode == PlayMode.Sequential)
        {
            currentSongIndex = (currentSongIndex + 1) % backgroundMusic.Count;
        }
        else
        {
            currentSongIndex = Random.Range(0, backgroundMusic.Count);
        }
        PlayerPrefs.SetInt("CurrentSongIndex", currentSongIndex);
        audioSource.clip = backgroundMusic[currentSongIndex];
        audioSource.Play();
    }    
    private void InitializeDropdown()
    {
        playModeDropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("Sequential"),
            new TMP_Dropdown.OptionData("Random")
        };
        playModeDropdown.AddOptions(options);
        playModeDropdown.value = (int)playMode;
    }
    private void InitializeSliders()
    {
        float masterVolume;
        if (audioMixer.GetFloat("MasterVolume", out masterVolume))
            masterSlider.value = DbToSliderValue(masterVolume);

        float bgmVolume;
        if (audioMixer.GetFloat("BGMVolume", out bgmVolume))
            bgmSlider.value = DbToSliderValue(bgmVolume);

        float sfxVolume;
        if (audioMixer.GetFloat("SFXVolume", out sfxVolume))
            sfxSlider.value = DbToSliderValue(sfxVolume);

        float speechVolume;
        if (audioMixer.GetFloat("SpeechVolume", out speechVolume))
            speechSlider.value = DbToSliderValue(speechVolume);

        float voiceVolume;
        if (audioMixer.GetFloat("SFXVolume", out voiceVolume))
            voiceSlider.value = DbToSliderValue(voiceVolume);
    }
    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", SliderValueToDb(value));
    }
    private void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", SliderValueToDb(value));
    }
    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", SliderValueToDb(value));
    }
    private void SetSpeechVolume(float value)
    {
        audioMixer.SetFloat("SpeechVolume", SliderValueToDb(value));
    }
    private void SetVoiceVolume(float value)
    {
        audioMixer.SetFloat("VoiceVolume", SliderValueToDb(value));
    }
    private float DbToSliderValue(float db)
    {
        return Mathf.InverseLerp(minDb, maxDb, db) * 100f;
    }
    private float SliderValueToDb(float value)
    {
        return Mathf.Lerp(minDb, maxDb, value / 100f);
    }
    public void ResetToDefault()
    {
        audioMixer.SetFloat("MasterVolume", 0f);
        audioMixer.SetFloat("BGMVolume", 0f);
        audioMixer.SetFloat("SFXVolume", 0f);
        audioMixer.SetFloat("SpeechVolume", 0f);
        audioMixer.SetFloat("VoiceVolume", 0f);

        masterSlider.value = DbToSliderValue(0f);
        bgmSlider.value = DbToSliderValue(0f);
        sfxSlider.value = DbToSliderValue(0f);
        speechSlider.value = DbToSliderValue(0f);
        voiceSlider.value = DbToSliderValue(0f);
    }
}