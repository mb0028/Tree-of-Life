using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnPoint : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private AudioSource sfx;



    void Awake()
    {
        try { sfx = GameObject.FindGameObjectWithTag("ButtonsSound").GetComponent<AudioSource>(); }
        catch(System.Exception) { }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sfx != null) { sfx.PlayOneShot(sfx.clip); }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (sfx != null) { sfx.PlayOneShot(sfx.clip); }
    }
}
