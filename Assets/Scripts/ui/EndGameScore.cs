using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndGameScore : MonoBehaviour
{
    private TextMeshProUGUI scoreEndText;
    public TextMeshProUGUI endlessExtraEndText;
    public GameObject endlessCardsScrollbar;

    private Volume volume;
    private DepthOfField depthOfField;

    private int thisScene;
    void Awake()
    {
        thisScene = SceneManager.GetActiveScene().buildIndex;
        if (thisScene == 1) endlessCardsScrollbar.SetActive(true);
        scoreEndText = GetComponent<TextMeshProUGUI>();    
        volume = FindFirstObjectByType<Volume>();
        if (volume != null) volume.profile.TryGet(out depthOfField);
        
        #if UNITY_ANDROID
        if (Setting.settingsData.vibrate) { Handheld.Vibrate(); }
        #endif
    }

    void OnEnable()
    {
        try {
            if (Setting.settingsData.effectsLevel >= 3) { depthOfField.mode.value = DepthOfFieldMode.Gaussian; } 
        } catch (System.Exception e) { Debug.LogWarning(e); }
    }
    void OnDisable()
    {
        try {
            if (Setting.settingsData.effectsLevel >= 3) { depthOfField.mode.value = DepthOfFieldMode.Off; } 
        } catch (System.Exception e) { Debug.LogWarning(e); }
    }


    void Update()
    {
        switch (thisScene)
        {
            case 1:
                string VSRGMode = false ? $"\nVSRG Mode\nBad:  | Ok: | Perfect:" : "";
                scoreEndText.text = $"Score: {MusicCircleSpawner.Instance.score}\nSpawnrate: {MusicCircleSpawner.Instance.DifficultySlider.value}\nShrink Speed: {MusicCircleSpawner.Instance.shrinkSpeed}{VSRGMode}\n\n{$@"Music: {AudioSelector.trackName}"}";
                break;
            case 2: scoreEndText.text = $"Score: {CGMSpawner.Instance.kills}"; break;
            default: scoreEndText.text = $"Score: Score"; break;
        }
    }

}
