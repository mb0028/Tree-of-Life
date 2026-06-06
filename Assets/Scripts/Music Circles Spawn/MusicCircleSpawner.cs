using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicCircleSpawner : MonoBehaviour
{
    public static MusicCircleSpawner Instance;

    public GameObject circles;
    public TextMeshProUGUI lyrics;

    [Header("Circle Settings")]
    public float baseScale;
    public float shrinkSpeed;
    public float BandTrargetToSpawn;
    public float timeBetweenEverySpawn;
    public bool IsStarted = false;
    public bool isLiveRecording = false;


    [Header("Spawn Area")]
    public float areaX = -7f;
    public float areaXMax = 7f;
    public float areaY = -7f;
    public float areaYMax = 7f;

    #region Refs
    [Header("References")]
    public GameObject complatedPanel;
    public GameObject SelectUI;
    public GameObject[] tree;
    public Toggle showTree;
    public Toggle demoMode;
    public Slider DifficultySlider;
    public Slider ShrinkRateSlider;
    public TextMeshProUGUI LogText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI musicTime;
    public TextMeshProUGUI musicName;

    [Header("More")]
    public float score;
    public Button startBTN;
    public AudioSource audioSource;
    public AudioMixerGroup mixerG;
    public Image cover;
    public float band1;
    public float band2;
    public float band4;
    public float band6;

    private float spawnX;
    private float spawnY;
    private float t;
    private float PlayingTime;

#endregion

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (Application.platform == RuntimePlatform.WindowsEditor)
            if (File.Exists(Setting.SettingsPath))
                JsonConvert.PopulateObject(File.ReadAllText(Setting.SettingsPath), Setting.settingsData);
        
        showTree.onValueChanged.AddListener(ChangeTreeVisiblity);
        demoMode.onValueChanged.AddListener(ChangeDemoMode);
        DifficultySlider.onValueChanged.AddListener(DifficultValChange);
        ShrinkRateSlider.onValueChanged.AddListener(ShrinkSpeedChange);
        DifficultValChange(Setting.settingsData.music_difficult);
        ShrinkSpeedChange(Setting.settingsData.music_shrinkSpeed);
        ChangeTreeVisiblity(Setting.settingsData.music_treeVisibility);

        score = 0;
        scoreText.text = "Score: " + score;
    }

    void Update()
    {
        if (IsStarted)
        {
            t += Time.deltaTime;
            PlayingTime += Time.deltaTime;

            band1 = AudioPeer.audioBandBuffer[1];
            band2 = AudioPeer.audioBandBuffer[2];
            band4 = AudioPeer.audioBandBuffer[4];
            band6 = AudioPeer.audioBandBuffer[6];

            if (PlayingTime >= AudioSelector.trackDuration + 1f)
                complatedPanel.SetActive(true);
            else if (PlayingTime <= AudioSelector.trackDuration)
            {
                if (t >= timeBetweenEverySpawn && band1 >= BandTrargetToSpawn || t >= timeBetweenEverySpawn && band2 >= BandTrargetToSpawn ||
                t >= timeBetweenEverySpawn && band4 >= BandTrargetToSpawn || t >= timeBetweenEverySpawn && band6 >= BandTrargetToSpawn)
                {
                    Spawner();
                    t = 0f;
                }
            }
        }

        if (AudioSelector.canPlay) startBTN.interactable = true;
        else startBTN.interactable = false;

        if (audioSource.clip != null && AudioSelector.hasLRC)
            lyrics.text = AudioSelector.lrcParser.LineByAudioPosition(audioSource.time);

        if (audioSource.clip != null && isLiveRecording == false)
        {
            System.TimeSpan time = System.TimeSpan.FromSeconds(audioSource.time);
            musicTime.text = $"{string.Format("{0}:{1:D2}", (int)time.TotalMinutes, time.Seconds)} / {AudioSelector.trackDurationText}";
            musicName.text = AudioSelector.trackName;
        }
    }

    public async void StartTheGame()
    {
        await AudioSelector.ReadyTrack();
        audioSource.clip = AudioSelector.clip;
        IsStarted = true;
        LightIntensityMusic.started = true;

        if (isLiveRecording)
        {
            musicTime.gameObject.SetActive(false);
            AudioSelector.trackName = "Live (Microphone)";
            musicName.text = "Live (Microphone)";
        }

        SelectUI.SetActive(false);
        audioSource.Play();
    }
    
    public void Lose()
    {
        audioSource.VolumeTo(0f, 1.5f);
        //audioSource.Stop();
        IsStarted = false;
        uiButtons.Instance.GameOverpanel();
    }

    private void Spawner()
    {
        spawnX = Random.Range(areaX, areaXMax);
        spawnY = Random.Range(areaY, areaYMax);
        Instantiate(circles, new Vector3(spawnX, spawnY, transform.position.z), transform.rotation);
    }

    public void AddLog(string TheLog) => Debug.Log(TheLog);

    private void DifficultValChange(float value)
    {
        float rounded = RoundToTwoFloats(value);
        timeBetweenEverySpawn = rounded;
        DifficultySlider.value = rounded;
        string display = rounded == 0.09f ? "0.09 (Default)" : rounded.ToString();
        DifficultySlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Spawn Rate: {display}";
        Setting.settingsData.music_difficult = rounded;
        Setting.SaveSettings();
    }

    private void ShrinkSpeedChange(float value)
    {
        float rounded = RoundToTwoFloats(value);
        shrinkSpeed = rounded;
        ShrinkRateSlider.value = rounded;
        string display = rounded <= 0.11f ? "0.11 (Default)" : rounded.ToString();
        ShrinkRateSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Circles Shrink Speed: {display}";
        Setting.settingsData.music_shrinkSpeed = rounded;
        Setting.SaveSettings();
    }

    private void ChangeTreeVisiblity(bool value)
    {
        showTree.isOn = value;
        for (int i = 0; i < tree.Length; i++) { tree[i].SetActive(value); }
        Setting.settingsData.music_treeVisibility = value;
        Setting.SaveSettings();
    }

    private void ChangeDemoMode(bool value)
    {
        demoMode.isOn = value;
        BandTrargetToSpawn = value ? float.PositiveInfinity : 0.7f;
        score = value ? -1 : 0;
        scoreText.text = value ? "Demo Mode" : "Score: 0";
    }

    private float RoundToTwoFloats(float value)
    {
        float tempValue = value * 100f;
        tempValue = Mathf.Round(tempValue);
        float roundedValue = tempValue / 100f;
        return roundedValue;
    }

}


