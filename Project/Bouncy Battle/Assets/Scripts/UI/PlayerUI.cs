using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class SettingsData //Used for storing Username and total kills.
{
    public int ScaleIndex;
    public float Volume;

    public SettingsData()
    {
        this.ScaleIndex = 0;
        this.Volume = 1f;
    }

    public SettingsData(int i, float v)
    {
        this.ScaleIndex = i;
        this.Volume = v;
    }
}
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    GameObject scoreboard;
    [SerializeField]
    GameObject pausemenu;

    [SerializeField]
    GameObject settings;
    [SerializeField]
    GameObject gamemenu;

    [SerializeField]
    Text scaleButtonText;


    [SerializeField]
    CanvasScaler canvasScaler;

    public AudioMixer audioMixer;

    public static SettingsData _settings = new SettingsData();
    private float[] scaleMulti = { 1.25f, 1f, 0.75f };
    private string[] scaleText = { "Small", "Medium", "Large" };
    private int scaleIndex = 1;
    private float volumeValue;

    private void OnEnable()
    {
        _settings = Data.LoadSettings();
        canvasScaler.referenceResolution = new Vector2(1920*scaleMulti[_settings.ScaleIndex], 1080*scaleMulti[_settings.ScaleIndex]);
        audioMixer.SetFloat("volume", _settings.Volume);
    }

    private void OnDisable()
    {
        SettingsData _settingsData = new SettingsData(scaleIndex, volumeValue);
        Data.SaveSettings(_settingsData);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && pausemenu.activeInHierarchy != true)
        {
            OpenPause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausemenu.activeInHierarchy != false)
        {
            ClosePause();
        }
    }

    public void ResumePause()
    {
        pausemenu.SetActive(false);
    }

    public void ChangeUIScale()
    {
        scaleIndex++;
        if (scaleIndex >= scaleMulti.Length)
        {
            scaleIndex = 0;
        }
        canvasScaler.referenceResolution = new Vector2(1920 * scaleMulti[scaleIndex], 1080 * scaleMulti[scaleIndex]);
        scaleButtonText.text = "UI Size: " + scaleText[scaleIndex];
    }

    public void SetVolume(float volume)
    {
        volumeValue = volume;
        audioMixer.SetFloat("volume", volume);
    }

    public void OpenGamemenu()
    {
        CloseAll();
        gamemenu.SetActive(true);
    }

    public void OpenSettings()
    {
        CloseAll();
        settings.SetActive(true);
    }

    public void CloseAll()
    {
        settings.SetActive(false);
        gamemenu.SetActive(false);
    }

    public void OpenPause()
    {
        pausemenu.SetActive(true);
        OpenGamemenu();
    }

    public void ClosePause()
    {
        CloseAll();
        pausemenu.SetActive(false);
    }
}
