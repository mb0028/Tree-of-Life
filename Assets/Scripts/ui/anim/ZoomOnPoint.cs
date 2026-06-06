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
        transform.ScaleTo(startScale * zoomSizeMultiply, duration, EaseMode.EaseOutBack).Unscaled();
        PlaySFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.ScaleTo(startScale, duration, EaseMode.EaseOutBack).Unscaled();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.ScaleTo(startScale * 0.9f, duration, EaseMode.EaseOutBack).Unscaled();
        PlaySFX();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.ScaleTo(startScale, duration, EaseMode.EaseOutBack).Unscaled();
    }

    private void PlaySFX()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return;
        PauseTheGame.clickSFX.PlayOneShot(PauseTheGame.clickSFX.clip);
    }
}
