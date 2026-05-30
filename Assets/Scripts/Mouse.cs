using UnityEngine;
using UnityEngine.InputSystem;

public class MousePositionController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public GameObject MouseEffects;
    private Vector2 currentMousePosition;

    private void Update()
    {
        currentMousePosition = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            currentMousePosition = Touchscreen.current.primaryTouch.position.value;
        else if (Mouse.current != null)
            currentMousePosition = Mouse.current.position.value;

        if (currentMousePosition != Vector2.zero && mainCamera != null)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, mainCamera.nearClipPlane));
            MouseEffects.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);
        }
    }

}