using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomOnPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float zoomSizeMultiply = 1.1f;
    public float duration = 0.3f;

    private Vector3 startScale;
    void Start()
    {
        if (transform != null)
            startScale = transform.localScale;
        else
            startScale = Vector3.one;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = startScale * zoomSizeMultiply;
        PlaySFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = startScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = startScale * 0.9f;
        PlaySFX();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = startScale;
    }
    private void PlaySFX()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return;
        PauseTheGame.clickSFX.PlayOneShot(PauseTheGame.clickSFX.clip);
    }
}
