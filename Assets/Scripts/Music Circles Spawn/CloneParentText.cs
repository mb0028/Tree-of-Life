using UnityEngine;
using TMPro;

public class CloneTMPText : MonoBehaviour
{
    public TextMeshProUGUI thisTMP;
    public TextMeshProUGUI other;

    void Update()
    {
        thisTMP.text = other.text;
    }

}
