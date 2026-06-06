using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnPoint : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PauseTheGame.clickSFX.PlayOneShot(PauseTheGame.clickSFX.clip);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PauseTheGame.clickSFX.PlayOneShot(PauseTheGame.clickSFX.clip);
    }
}
