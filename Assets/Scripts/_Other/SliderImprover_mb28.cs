using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderImprover_mb28 : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI valueDisplay;
    public Button next;
    public Button pervious;
    public float nextPerviousValueChange = 1;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateSliderDisplay);
        UpdateSliderDisplay(slider.value);
    }

    private void UpdateSliderDisplay(float value)
    {
        int displayVal = Mathf.RoundToInt(Mathf.InverseLerp(slider.minValue, slider.maxValue, value) * 100);
        valueDisplay.text = displayVal.ToString();
    }
}
