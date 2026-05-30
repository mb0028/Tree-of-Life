using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseTheGame : MonoBehaviour
{
    public static PauseTheGame Instance;
    public GameObject pausePanel;
    public GameObject settingPanel;
    public bool isTVPlatform;

    public InputActionAsset inputActions;
    private InputAction PauseMenu;
    public static AudioSource clickSFX;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); } else { Destroy(gameObject); return; }

        PauseMenu = inputActions.FindAction("UI/Pause");
        PauseMenu.performed += context => PauseEsc();
        clickSFX = GetComponent<AudioSource>();
    }

    private void PauseEsc()
    {
        if (pausePanel.activeInHierarchy)
            _Resume();
        else
            _Pause();
    }

    public void ShowSettingsPanel() => settingPanel.SetActive(!settingPanel.activeSelf);

    public void _Pause()
    {
        Debug.Log("Paused ||");
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("PauseableMusic").GetComponent<AudioSource>().Pause();
    }


    public void _Resume()
    {
        Debug.Log("Resumes |>");
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("PauseableMusic").GetComponent<AudioSource>().UnPause();
    }

    public async void _MainMenu()
    {
        _Resume();
        CircleTransi.Instance.In();
        await Task.Delay(1000);
        SceneManager.LoadScene(0);
    }

    public async void _Collapse()
    {
        _Resume();
        CircleTransi.Instance.In();
        await Task.Delay(1000);
        Application.Quit();
    }

    private void OnEnable()
    {
        PauseMenu.Enable();
    }

    private void OnDisable()
    {
        PauseMenu.Disable();
    }
}
