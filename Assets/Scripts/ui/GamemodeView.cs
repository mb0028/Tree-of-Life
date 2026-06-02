using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;

public class GamemodeView : MonoBehaviour
{
    public Transform cardsList;
    public GameObject cardTemp;

    async void Awake()
    {
        using AndroidJavaClass mbJava = new("com.mb28.treeoflife.MBJava");
        if (mbJava.CallStatic<bool>("canManageMedia") == false)
        {
            var activity = AndroidApplication.currentActivity;
            //mbJava.CallStatic("ShowToast", activity, "Tree of Life needs all files access for modding and reading audio files");
            mbJava.CallStatic("GoToAllFilesAccess", activity);
            Application.Quit();
            return;
        }

        try {
            string[] mods = Directory.GetDirectories($"{CustomGameLoader.ModsPath}");
            foreach (var modDir in mods)
            {
                GameObject dgDLC = Instantiate(cardTemp, cardsList);
                var info = dgDLC.GetComponent<GamemodeCard>();
                info.displayName = $"{Path.GetFileName(modDir)} (Mod)";
                info.fadeColor = Color.brown;
                info.banner = await GetSpriteNull($"{modDir}/cover.png");
                info.sceneIndex = 2;
                info.modName = Path.GetFileName(modDir);
                dgDLC.GetComponent<TooltipItem>().TooltipText = File.ReadAllText($"{modDir}/info.txt");
            }
        }
        catch (System.Exception)
        {
        }
    }
    
    private async Task<Sprite> GetSpriteNull(string filePath)
    {
        if (File.Exists(filePath))
        {
            Texture2D tex = new(2, 2);
            if (tex.LoadImage(await File.ReadAllBytesAsync(filePath)))
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        return null;
    }

}
