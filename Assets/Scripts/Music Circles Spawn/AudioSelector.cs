using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MB28.Music;
using TagLib.Id3v2;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioSelector : MonoBehaviour
{
    public Image cover;
    public TextMeshProUGUI currentPathTMP;
    public AudioSource source;
    
    public static AudioClip clip;
    public static float trackDuration;
    public static string trackDurationText;
    public static string trackName;
    public static bool canPlay = false;
    public static bool hasLRC = false;
    public static LRCParser lrcParser;

    public static string selectedAudioPath;
    private string ChangeSelectedAudioPath
    {
        set
        {
            selectedAudioPath = value;
            currentPathTMP.text = value;
            Setting.settingsData.music_lastTrackPath = value;
            Setting.SaveSettings();

            string lrcPath = $"{Path.GetDirectoryName(value)}/{Path.GetFileNameWithoutExtension(value)}.lrc";
            if (File.Exists(lrcPath))
            {
                lrcParser = new LRCParser(lrcPath, 250);
                hasLRC = true;
            }
            else
            {
                lrcParser = null;
                hasLRC = false;
            }
        }
    }
    private readonly List<string> tracksPaths = new();

    
    void Awake()
    {
        if (!string.IsNullOrWhiteSpace(Setting.settingsData.music_lastTrackPath))
            ChangeSelectedAudioPath = Setting.settingsData.music_lastTrackPath;

        Task.Run(LoadTracks);
    }

    public void SelectRandomAudio()
    {
        if (canPlay)
            SelectAudio(Random.Range(0, tracksPaths.Count));
    }

    public void SelectAudio(int tracksPathsIndex)
    {
        if (canPlay)
            ChangeSelectedAudioPath = tracksPaths[tracksPathsIndex];
    }

    public void PickAudio()
    {
        if (canPlay)
            NativeFilePicker.PickFile(async i =>
            {
                ChangeSelectedAudioPath = i;
                await ReadyTrack();
            }, "mp3");
    }

    public static async Task ReadyTrack()
    {
        if (string.IsNullOrWhiteSpace(selectedAudioPath) || !File.Exists(selectedAudioPath))
            return;
        try
        {
            Tag frames = new(new TagLib.Mpeg.File(selectedAudioPath), 0, TagLib.ReadStyle.PictureLazy);
            trackName = $"{frames.Performers[0]} • {frames.Title} • {frames.Year}";
            
            Texture2D texture = new(2, 2);
            byte[] bytes = new byte[frames.Pictures[0].Data.Count];
            frames.Pictures[0].Data.CopyTo(bytes, 0);
            texture.LoadImage(bytes);
        }
        catch (System.Exception) { }

        DownloadHandlerAudioClip downloader = new($"File://{selectedAudioPath}", AudioType.MPEG) { streamAudio = true };
        UnityWebRequest web = new($"File://{selectedAudioPath}", "GET", downloader, null);

        await web.SendWebRequest();
        clip = DownloadHandlerAudioClip.GetContent(web);
        web.Dispose();

        trackDuration = clip.length;
        System.TimeSpan t = System.TimeSpan.FromSeconds(trackDuration);
        trackDurationText = string.Format("{0}:{1:D2}", (int)t.TotalMinutes, t.Seconds);
    }

    public Task LoadTracks()
    {
        var d = CommonMusicDirs();
        for (int i = 0; i < d.Length; i++)
            foreach (var item in Directory.GetFiles(d[i], "*.mp3", SearchOption.AllDirectories))
                tracksPaths.Add(item);
        canPlay = true;
        return Task.CompletedTask;
    }
    
    private string[] CommonMusicDirs()
    {
        var t = new List<string>();
        if (Application.platform == RuntimePlatform.Android)
        {
            t.Add("/sdcard/Download");
            t.Add("/sdcard/Music");
        }
        else
        {
            t.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
            t.Add(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic));
        }
        return t.ToArray();
    }
}
