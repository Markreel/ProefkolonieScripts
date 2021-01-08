using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] AudioClip music;
    [SerializeField] GameObject resetSaveFileWindow;
    [SerializeField] GameObject codeWindow;

    [Header("Geheime code settings: ")]
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] GameObject correctCodeWindow;
    [SerializeField] GameObject codeResetWindow;
    [SerializeField] string code = "Er gaat een wereld voor je open";

    AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.ignoreListenerPause = true;
        audioSource.clip = music;
        audioSource.Play();
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OpenResetSaveFileWindow()
    {
        resetSaveFileWindow.SetActive(true);
    }

    public void ProceedWithReset()
    {
        SaveSystem.DeleteSaveFile();
        resetSaveFileWindow.SetActive(false);
    }

    public void CancelReset()
    {
        resetSaveFileWindow.SetActive(false);
    }

    public void OpenCodeWindow()
    {
        codeWindow.SetActive(true);
    }

    public void CloseCodeWindow()
    {
        codeWindow.SetActive(false);
        correctCodeWindow.SetActive(false);
        codeResetWindow.SetActive(false);
    }

    public void CheckCode()
    {
        if (codeInputField.text.Equals(code, StringComparison.InvariantCultureIgnoreCase))
        {
            SaveSystem.ActivateEasterEgg(true);
            codeWindow.SetActive(false);
            correctCodeWindow.SetActive(true);
        }
    }

    public void ResetCode()
    {
        SaveSystem.ActivateEasterEgg(false);
        codeWindow.SetActive(false);
        codeResetWindow.SetActive(true);
    }

    public void ClickOnKoalaFishLogo()
    {
        Application.OpenURL("https://koalafish-studios.itch.io/");
    }

    public void ClickOnMuseumLogo()
    {
        Application.OpenURL("https://proefkolonie.nl/");
    }
}
