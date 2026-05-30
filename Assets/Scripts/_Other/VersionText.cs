using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    public TextMeshProUGUI tmpUI;
    void Awake()
    {
        tmpUI.text = $"v {Application.version}";
    }
    
}
