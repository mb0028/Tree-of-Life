using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLevelGetter : MonoBehaviour
{
    public static BatteryLevelGetter Instance;

    public TextMeshProUGUI batterylevelTMP;
    public Slider batterySlider;
    private float batterylevel;
    private float time = 5;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); return; }
    }

    void OnEnable()
    {
        if (SystemInfo.batteryLevel == -1 || PauseTheGame.Instance.isTVPlatform)
        {
            batterylevelTMP.text = "100";
            batterySlider.value = 100;
            Destroy(this);
            return;
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 5)
        {
            batterylevel = Mathf.Round(SystemInfo.batteryLevel * 100);
    
            batterylevelTMP.text = batterylevel.ToString();
            batterySlider.value = batterylevel;
            time = 0;
        }
    }



}
