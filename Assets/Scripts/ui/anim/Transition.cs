using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class CircleTransi : MonoBehaviour
{
    public static CircleTransi Instance;
    public Material m;
    public CanvasGroup c;
    public Vector3 startT;

    private readonly int borderID = Shader.PropertyToID("_Border");
    private readonly int insideID = Shader.PropertyToID("_Inside");

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); return; }
        c = GetComponent<CanvasGroup>();
        SceneManager.activeSceneChanged += Outt;
        startT = transform.position;
    }

    private void Outt(Scene arg0, Scene arg1)
    {
        AnimOut();
    }

    // Transition are inspired by website TODO: ADD WEB URL
    public async void In()
    {
        Vector2 currentMousePosition = Vector2.zero;
        if (Touchscreen.current != null)
        {
            currentMousePosition = Touchscreen.current.primaryTouch.position.value;
        }
        else if (Mouse.current != null)
        {
            currentMousePosition = Mouse.current.position.value;
        }
        transform.position = currentMousePosition;

        c.blocksRaycasts = true;
        m.FloatTo01(borderID, 1f, 0.6f);
        await Task.Delay(400);
        m.FloatTo01(insideID, 1f, 0.6f);
    }

    public void AnimOut()
    {
        transform.position = startT;
        c.blocksRaycasts = false;
        m.FloatTo01(borderID, 0f, 0.4f);
        m.FloatTo01(insideID, 0f, 0.3f);
    }
   
}
