using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private float fps;
    private float t;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        t += Time.deltaTime;
        fps++;
        if (t > 1)
        {
            if (fps < 30) { tmp.text = $"<color #ff0000>{fps}</color>"; }
            else if (fps >= 30 && fps < 60) { tmp.text = $"<color #b6c300ff>{fps}</color>"; }
            else { tmp.text = $"{fps}"; }
            t = 0f;
            fps = 0;
        }
    }
}
