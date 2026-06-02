using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GamemodeCard : MonoBehaviour
{
    public int sceneIndex;
    public string modName;
    public string displayName;
    public Sprite banner;
    public Color fadeColor;

    void Start()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(async () => {
            CustomGameLoader.gamemodeToLoad = modName;
            CircleTransi.Instance.In();
            await Task.Delay(1000);
            await SceneManager.LoadSceneAsync(sceneIndex);
        });
        
        GetComponentInChildren<TextMeshProUGUI>().text = displayName;
        transform.Find("Portrait").GetComponent<Image>().sprite = banner;
        transform.Find("Fade").GetComponent<Image>().color = fadeColor;
    }

    void OnValidate() => Start();

}
