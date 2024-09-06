using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Panels")]
    public GameObject exitPanel;
    public GameObject loadPanel;
    public Slider loadSlider;
    public AsyncOperation loadAsync;    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (loadAsync != null)
        {
            loadSlider.value = Mathf.MoveTowards(loadSlider.value, loadAsync.progress, 50 * Time.deltaTime);
        }
    }
    public void LoadScene(string nameLevel)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(nameLevel));
    }
    IEnumerator LoadAsync(string nameLevel)
    {
        loadAsync = SceneManager.LoadSceneAsync(nameLevel);
        while (!loadAsync.isDone)
        {
            yield return null;
        }
        loadPanel.SetActive(false);
    }
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}