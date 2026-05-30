using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class uiButtons : MonoBehaviour
{
    public static uiButtons Instance;


    [Header("menus")]
    public GameObject Debugtext;
    public GameObject LPanel;
    public GameObject MouseOff;
    public GameObject DontSpawn;


    [Tooltip("The Input Actions asset")]
    public InputActionAsset inputActions;
    private InputAction hideUI;
    private InputAction debugTopLeftTexts;
    private bool hideUIBool = false;


    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        debugTopLeftTexts = inputActions.FindAction("UI/DebugUI");
        hideUI = inputActions.FindAction("UI/F1");
        debugTopLeftTexts.performed += context => ToggleDebugUI();
        hideUI.performed += ToggleHideUI;
    }

    private void ToggleHideUI(InputAction.CallbackContext context)
    {
        Canvas[] canvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].GetComponent<CanvasGroup>() == null)
                canvas[i].gameObject.AddComponent<CanvasGroup>();
            canvas[i].GetComponent<CanvasGroup>().alpha = hideUIBool ? 1.0f : 0.0f;
        }
        hideUIBool = !hideUIBool;
    }

    public void ToggleDebugUI()
    {
        if (Debugtext != null && Debugtext.activeInHierarchy == false)
        {
            Debugtext.SetActive(true);
        }
        else if (Debugtext != null && Debugtext.activeInHierarchy == true)
        {
            Debugtext.SetActive(false);
        }
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public async void BackToMM()
    {
        CircleTransi.Instance.In();
        await Task.Delay(1000);
        SceneManager.LoadScene(0);
    }

    public void GameOverpanel()
    {
        if (LPanel != null) LPanel.SetActive(true);

        if (MouseOff != null) MouseOff.SetActive(false);

        if (DontSpawn != null) DontSpawn.SetActive(false);
    }

    public void MusicRefresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnEnable()
    {
        debugTopLeftTexts.Enable();
    }

    private void OnDisable()
    {
        debugTopLeftTexts.Disable();
    }

}
