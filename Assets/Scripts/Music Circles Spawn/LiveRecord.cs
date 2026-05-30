using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class LiveRecord : MonoBehaviour
{
    public AudioSource audioSource;
    public string micName;
    public bool nullMic = false;
    public AudioMixer mixer;
    WebCamTexture cam;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices.FirstOrDefault();
            MusicCircleSpawner.Instance.AddLog($"<color #b6b6b6ff>Mic Ready: {micName}</color>");
        }
        else {
            nullMic = true;
        }
    }

    public void testss(ref Texture2D image)
    {
        
    }
    
    public void Rec()
    {
        if (nullMic) { MusicCircleSpawner.Instance.AddLog("<color #ff0000ff>No microphone finded</color>"); Start(); return; }

        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Mute").FirstOrDefault();
        audioSource.clip = Microphone.Start(micName, true, 1, 44100);
        while (!(Microphone.GetPosition(micName) > 0)) { }

        MusicCircleSpawner.Instance.isLiveRecording = true;
        MusicCircleSpawner.Instance.StartTheGame();
    }

    public void RecStop()
    {
        Microphone.End(micName);
        audioSource.loop = false;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("music").FirstOrDefault();
        audioSource.volume = 1;
        MusicCircleSpawner.Instance.isLiveRecording = false;
        MusicCircleSpawner.Instance.IsStarted = false;
    }
    [ContextMenu("Save As Wave")]
    public void RecSaveAsWave()
    {
        NativeFilePicker.ExportFile(Application.persistentDataPath + "/Changelog.txt", callback =>
        {
            if (callback) { TreeNotification.ScreenshotNotif("Saved!"); }
        });
    }
}
