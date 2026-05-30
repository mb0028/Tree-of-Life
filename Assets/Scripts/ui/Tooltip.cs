using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour
{
    public Camera mainCamera;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI tmp;
    public Vector2 offset;

    public static bool showTooltip = false;
    public static Color textColor = Color.white;
    public static string tooltipText = "ERROR";

    public Vector3 mousePos;

    void Update()
    {
        if (showTooltip) OnShowTooltip();
        else { canvasGroup.alpha = 0.0f; return; }
        
        if (mainCamera == null) mainCamera = Camera.main;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            mousePos = Touchscreen.current.primaryTouch.position.value;
        else if (Touchscreen.current != null && !Touchscreen.current.primaryTouch.press.isPressed)
            mousePos = Vector2.zero - (offset * 3);
        else if (Mouse.current != null)
            mousePos = Mouse.current.position.value;

        
        if (mousePos.y > 900)
            transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + -offset.y, transform.position.z);
        else
            transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
    }
    
    public void OnShowTooltip()
    {
        canvasGroup.alpha = 1.0f;
        tmp.text = tooltipText;
        tmp.color = textColor;
    }
}
