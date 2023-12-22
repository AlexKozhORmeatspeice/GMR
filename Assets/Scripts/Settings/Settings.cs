using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private TMP_Text textTMP;
    [SerializeField] private Slider volumeSl;
    [SerializeField] private Slider mouseSenseSl;
    
    private string text = "Shift - run\nLMouse - shoot\nWasd - move ";

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }


        volumeSl.value = PlayerPrefs.GetFloat("Volume"); 
        
        mouseSenseSl.value = PlayerPrefs.GetFloat("MouseSense");

    }

    public void LoadGame()
    {
        if (textTMP != null)
        {
            textTMP.text = "";
            textTMP.text = text;
        }
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsWindow.SetActive(false);
    }
    
    public void CloseGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenMenu()
    {
        print(1);
        SceneManager.LoadScene("Menu");
    }

    public void SetVolume(float t)
    {
        PlayerPrefs.SetFloat("Volume", t);
    }
    
    public void SetMouseSens(float t)
    {
        PlayerPrefs.SetFloat("MouseSense", t);
    }
}
