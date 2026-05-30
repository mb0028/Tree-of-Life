using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomGameLoader : MonoBehaviour
{
    public Camera cameraaa;
    public GameObject clouds;
    public GameObject errorText;
    public GameObject startButton;
    public GameObject losePanel;
    public GameObject Spawner;
    public AudioSource audioSource;
    public SpriteRenderer bg;
    public SpriteRenderer tree;

    public static List<Enemy> enemies = new();
    public GameSettings gameSettings = new();

    public Sprite missing;
    public static bool isReady = false;

    public static string ModsPath => Application.platform == RuntimePlatform.Android ?
        "/sdcard/Documents/Tree Of Life" : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Tree Of Life Mods");

    public static string gamemodeToLoad = "";

    void Awake()
    {
        LoadGamemode(gamemodeToLoad);
    }

    private async void LoadGamemode(string name)
    {
        try
        {
            new DirectoryInfo(ModsPath).Create();
            enemies = new();
            gameSettings = new();
    
            string gmDir = Path.Combine(ModsPath, name);
            string gmEnemies = Path.Combine(gmDir, "Enemies");
            string gmManifest = $"{gmDir}/manifest.json";
            string gmBGM = $"{gmDir}/BGM.mp3";
            string gmWorld = $"{gmDir}/world.png";
            string gmTree = $"{gmDir}/tree.png";
    
            tree.sprite = await GetSpriteSafe(gmTree, 720, 720);
            bg.sprite = await GetSpriteSafe(gmWorld, 1920, 1080);
    
            JsonConvert.PopulateObject(await File.ReadAllTextAsync(gmManifest), gameSettings);
            clouds.SetActive(gameSettings.showClouds);
            cameraaa.backgroundColor = gameSettings.cameraBGColor;
            cameraaa.orthographicSize = gameSettings.cameraZoom;

            if (!gameSettings.useDefaultBGM)
                audioSource.clip = await GetAudio(gmBGM);
    
            string[] gmEnemiesData = Directory.GetFiles(gmEnemies, "*_Enemy.json");
            string[] gmEnemiesSprite = Directory.GetFiles(gmEnemies, "*_Enemy.png");

            for (int e = 0; e < gmEnemiesData.Length; e++)
            {
                EnemyData eed = new();
                JsonConvert.PopulateObject(await File.ReadAllTextAsync(gmEnemiesData[e]), eed);
                Enemy ee = new()
                {
                    enemyData = eed,
                    sprite = await GetSpriteSafe(gmEnemiesSprite[e], 720, 720)
                };
                enemies.Add(ee);
            }
            errorText.transform.parent.gameObject.SetActive(false);
            isReady = true;
        }
        catch (Exception e)
        {
            errorText.GetComponent<TextMeshProUGUI>().text = e.ToString();
            errorText.SetActive(true);
        }
    }

    public void StartGame()
    {
        Spawner.SetActive(isReady);
        losePanel.SetActive(false);
        startButton.SetActive(false);
        CGMSpawner.Instance.kills = 0;
        CGMSpawner.Instance.spawnrate = gameSettings.spawnRateEverySecond;
        CGMSpawner.Instance.treeHP = gameSettings.treeHP;
        CGMSpawner.Instance.StartCoroutine(CGMSpawner.Instance.SpawnEnemy());
        audioSource.volume = 1;
        audioSource.Play();
    }

    public async Task<Sprite> GetSpriteSafe(string filePath, int w, int h)
    {
        if (File.Exists(filePath))
        {
            Texture2D tex = new(2, 2);
            if (tex.LoadImage(await File.ReadAllBytesAsync(filePath)))
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        return missing;
    }

    private async Task<AudioClip> GetAudio(string fileName)
    {
        if (File.Exists(fileName))
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip($"File://{fileName}", AudioType.MPEG);
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                return DownloadHandlerAudioClip.GetContent(www);

            www.Dispose();
        }
        return null;
    }

    public void TryAgain() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    [Serializable]
    public class GameSettings
    {
        public int treeHP;
        public Color cameraBGColor;
        public float cameraZoom;
        public bool showClouds;
        public float spawnRateEverySecond;
        public bool useDefaultBGM = false;
    }
    
    [Serializable]
    public class EnemyData
    {
        public string name;
        public int minHP;
        public int maxHPExclusive;
        public float minSpeed;
        public float maxSpeed;
        public float minScale;
        public float maxScale;
        public float atkPower;
    }

    public class Enemy
    {
        public Sprite sprite;
        public EnemyData enemyData;
    }
}
