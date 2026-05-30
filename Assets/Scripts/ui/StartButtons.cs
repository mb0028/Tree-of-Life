using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    public TextMeshProUGUI splashTMP;
    public string[] splashTexts;
    public Animator aaA;
    public GameObject changelogPanel;
    public RectTransform canvasTransform;
    public GameObject HMMPanelPrefab;
    
    void Start()
    {
        splashTMP.text = splashTexts[Random.Range(0, splashTexts.Length)];
        if (PauseTheGame.Instance.isTVPlatform && Mouse.current == null)
            splashTMP.text = "Mouse Recommended";
    }

    public async void _exit()
    {
        CircleTransi.Instance.In();
        await Task.Delay(1000);
        Application.Quit();
    }

    public void _Collapse() => _exit();
    public void _StartClicked() => aaA.SetTrigger("start");
    public void _BackToStart() => aaA.SetTrigger("back");
    public void __GoToScene(int value) => PlayAnimator(value);
    public void ToggleChangelogPanel() => changelogPanel.SetActive(!changelogPanel.activeSelf);
    public void SpawnHMMPanel() => Instantiate(HMMPanelPrefab, canvasTransform);

 
    private async void PlayAnimator(int SceneIndex)
    {
        CircleTransi.Instance.In();
        await Task.Delay(1000);
        SceneManager.LoadScene(SceneIndex);
    }

    public void Insta() => Application.OpenURL("https://github.com/mb0028");
    public void X() => Application.OpenURL("https://www.x.com/mb_0028x");
    public void YT() => Application.OpenURL("https://www.youtube.com/mb_0028yt");
    public void UpdateFromDrive() => Application.OpenURL("");
}
