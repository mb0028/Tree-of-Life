using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LightIntensityMusic : MonoBehaviour
{
    public int band;

    public static bool started = false;

    private SpriteRenderer spriteRenderer;
    private Color color;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        started = false;
        color = spriteRenderer.color;
    }

    void Update()
    {
        if (started == false)
            return;

        // if (useBuffer)
            spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(AudioPeer.audioBandBuffer[band]));

        // if (!useBuffer)
        //     spriteRenderer.color = new(color.r, color.g, color.b, AudioPeer.audioBand[band]);
    }
    



}
