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

    public async void In()
    {
        Vector2 currentMousePosition = Vector2.zero;
        if (Touchscreen.current != null) {
            currentMousePosition = Touchscreen.current.primaryTouch.position.value;
        }
        else if (Mouse.current != null) {
            currentMousePosition = Mouse.current.position.value;
        }
        transform.position = currentMousePosition;

        c.blocksRaycasts = true;
        // m.DOFloat(1f, "_Border", 0.6f);
        // await Task.Delay(400);
        // m.DOFloat(1f, "_Inside", 0.6f);
    }

    public void AnimOut()
    {
        transform.position = startT;
        c.blocksRaycasts = false;
        // m.DOFloat(0f, "_Border", 0.4f);
        // m.DOFloat(0f, "_Inside", 0.4f);
    }
   
}
