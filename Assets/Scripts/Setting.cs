using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.IO;
using Newtonsoft.Json;

public class Setting : MonoBehaviour
{
    public static Setting Instance;

    public static SavedData settingsData = new();
    public AudioMixer mixer;

    [Header("Settings UI")]
    public TMP_Dropdown renderScale;
    public Toggle clouds;
    public TMP_Dropdown effects;
    public TMP_Dropdown toneMap;
    public Toggle vibrate;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public GameObject[] FPSButtons;

    [Header("URP")]
    public UniversalRenderPipelineAsset urp;
    public VolumeProfile volumeProfile;

    private Tonemapping tonemapping;
    private Bloom bloom;
    public static string SettingsPath => $"{Application.persistentDataPath}/settings.json";

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); return; }
    }

    void Start()
    {
        volumeProfile.TryGet(out tonemapping);
        volumeProfile.TryGet(out bloom);

        renderScale.onValueChanged.AddListener(RenderScale);
        toneMap.onValueChanged.AddListener(ToneMapping);
        clouds.onValueChanged.AddListener(ShowClouds);
        effects.onValueChanged.AddListener(EffectsLevel);
        vibrate.onValueChanged.AddListener(ShouldVibrate);
        MusicVolume.onValueChanged.AddListener(MusicVolumeSet);
        SFXVolume.onValueChanged.AddListener(SFXVolumeSet);

        if (File.Exists(SettingsPath))
            JsonConvert.PopulateObject(File.ReadAllText(SettingsPath), settingsData);
        else
            SaveSettings();
        
        RenderScale(settingsData.renderScaleLevel);
        ShowClouds(settingsData.showClouds);
        EffectsLevel(settingsData.effectsLevel);
        ToneMapping(settingsData.toneMappingMode);
        FPSLevel(settingsData.fpsLevel);
        MusicVolumeSet(settingsData.musicV);
        SFXVolumeSet(settingsData.sfxV);
        ShouldVibrate(settingsData.vibrate);
    }

    //-----------------------------------------------------------------------------------------------------------------

    public void RenderScale(int value)
    {
        renderScale.value = value;
        settingsData.renderScaleLevel = value;

        switch (value)
        {
            case 0: urp.renderScale = 1.0f; break;
            case 1: urp.renderScale = 0.8f; break;
            case 2: urp.renderScale = 0.75f; break;
            case 3: urp.renderScale = 0.5f; break;
            case 4: urp.renderScale = 0.25f; break;
            case 5: urp.renderScale = 0.1f; break;
        }
        SaveSettings();
    }

    public void ShowClouds(bool value)
    {
        clouds.isOn = value;
        settingsData.showClouds = clouds.isOn;

        SaveSettings();
    }

    public void EffectsLevel(int value)
    {
        effects.value = value;
        settingsData.effectsLevel = value;

        ChangeBloom();
        
        SaveSettings();
    }

    public void ToneMapping(int value)
    {
        toneMap.value = value;
        settingsData.toneMappingMode = value;

        tonemapping.mode.value = value == 1 ? TonemappingMode.Neutral : TonemappingMode.None;
        ChangeBloom();

        SaveSettings();
    }

    public void ShouldVibrate(bool value)
    {
        vibrate.isOn = value;
        settingsData.vibrate = vibrate.isOn;

        SaveSettings();
    }

    public void MusicVolumeSet(float value)
    {
        MusicVolume.value = value;
        settingsData.musicV = value;
        mixer.SetFloat("music", value);

        SaveSettings();
    }

    public void SFXVolumeSet(float value)
    {
        SFXVolume.value = value;
        settingsData.sfxV = value;
        mixer.SetFloat("sfx", value);

        SaveSettings();
    }

    public void FPSLevel(int value)
    {
        settingsData.fpsLevel = value;
        for (int i = 0; i < FPSButtons.Length; i++)
        {
            FPSButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            FPSButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        FPSButtons[value].GetComponent<Image>().color = Color.white;

        Application.targetFrameRate = value switch
        {
            0 => 30,
            1 => 60,
            2 => 120,
            _ => 120
        };

        SaveSettings();
    }

    private void ChangeBloom()
    {
        float bloomValue = tonemapping.mode.value == TonemappingMode.None ? 0.5f : 1.25f;
        float bloomIntensity = settingsData.effectsLevel >= 2 ? bloomValue : 0;
        bloom.intensity.value = bloomIntensity;
    }

    public void OpenMES()
    {
        using AndroidJavaClass mbJava = new("com.mb28.treeoflife.MBJava");
        mbJava.CallStatic("GoToAllFilesAccess", UnityEngine.Android.AndroidApplication.currentActivity);
    }

    public static void SaveSettings() => File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settingsData));

}
