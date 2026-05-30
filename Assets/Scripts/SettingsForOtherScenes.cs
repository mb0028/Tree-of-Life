using UnityEngine;

public class SettingsForOtherScenes : MonoBehaviour
{
    public GameObject Clouds;
    public GameObject Lights;

    void Update()
    {
        if (Clouds != null && Clouds.activeSelf != Setting.settingsData.showClouds)
            Clouds.SetActive(Setting.settingsData.showClouds);

        bool lightsCanBeActive = Setting.settingsData.effectsLevel >= 1;
        if (Lights != null && Lights.activeSelf != lightsCanBeActive)
            Lights.SetActive(lightsCanBeActive);
    }

}
