using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DebugUI : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private int ThisScene;
    private string GlobalDebugTexts = "";

    void Start()
    {
        ThisScene = SceneManager.GetActiveScene().buildIndex;
        GlobalDebugTexts = $"{SystemInfo.deviceModel}\n{SystemInfo.deviceType} | {SystemInfo.operatingSystem}\n{SystemInfo.processorModel}\n{SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsDeviceVendor}) v{SystemInfo.graphicsDeviceVersion}\n\n";
    }

    void Update()
    {
        if (this.gameObject.activeInHierarchy == false) { return; }
        debugText.text = ThisScene switch
        {
            2 => "Gamemode: " + GMsName.MusicSpawn + "\n"
                    + AllBands()
                    + $"Current Bands in use: 1, 2, 4, 6\nTarget Spawnrate (Red): {MusicCircleSpawner.Instance.BandTrargetToSpawn}",
            _ => $"Scene / Gamemode: {SceneManager.GetActiveScene().name}\n{GlobalDebugTexts}"
        };
    }

    private string AllBands()
    {
        string result = "";
        for (int i = 0; i <= 7; i++)
            result += AudioPeer.audioBandBuffer[i] < MusicCircleSpawner.Instance.BandTrargetToSpawn ? $"Band {i}: {AudioPeer.audioBandBuffer[i]}\n" : $"<color #ff0000>Band {i}: {AudioPeer.audioBandBuffer[i]}</color>\n";

        return result;
    }

}