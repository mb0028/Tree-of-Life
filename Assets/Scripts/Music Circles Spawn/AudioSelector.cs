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
    public static AudioSelector Instance;
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
        if (Instance == null) Instance = this;
        if (!string.IsNullOrWhiteSpace(Setting.settingsData.music_lastTrackPath))
        {
            ChangeSelectedAudioPath = Setting.settingsData.music_lastTrackPath;
            LoadTags();
        }
            

        Task.Run(LoadTracks);
    }

    public void SelectRandomAudio()
    {
        if (canPlay)
            SelectAudio(Random.Range(0, tracksPaths.Count - 1));
    }

    public void SelectAudio(int tracksPathsIndex)
    {
        if (canPlay)
        {
            ChangeSelectedAudioPath = tracksPaths[tracksPathsIndex];
            LoadTags();
        }
    }

    public void PickAudio()
    {
        if (canPlay)
            NativeFilePicker.PickFile(async i =>
            {
                ChangeSelectedAudioPath = i;
                LoadTags();
                await ReadyTrack();
            }, "mp3");
    }

    public static async Task ReadyTrack()
    {
        if (string.IsNullOrWhiteSpace(selectedAudioPath) || !File.Exists(selectedAudioPath))
            return;

        DownloadHandlerAudioClip downloader = new($"File://{selectedAudioPath}", AudioType.MPEG)
        {
            streamAudio = true
        };
        UnityWebRequest web = new($"File://{selectedAudioPath}", "GET", downloader, null);

        await web.SendWebRequest();
        clip = DownloadHandlerAudioClip.GetContent(web);
        web.Dispose();

        trackDuration = clip.length;
        System.TimeSpan t = System.TimeSpan.FromSeconds(trackDuration);
        trackDurationText = string.Format("{0}:{1:D2}", (int)t.TotalMinutes, t.Seconds);
    }

    private static void LoadTags()
    {
        var tags = TagLib.File.Create(selectedAudioPath);
        trackName = $"{tags.Tag.Title}\n{tags.Tag.Composers[0]} • {tags.Tag.Performers[0]} • {tags.Tag.Genres[0]} • {tags.Tag.Year}";
        MusicCircleSpawner.Instance.LogText.text = trackName;

        Texture2D img = new(2, 2);
        byte[] bytes = new byte[tags.Tag.Pictures[0].Data.Count];
        tags.Tag.Pictures[0].Data.CopyTo(bytes, 0);
        img.LoadImage(bytes);
        Instance.cover.sprite = Sprite.Create(img, new(0, 0, img.width, img.height), new(0.5f, 0.5f));
        tags.Dispose();
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
