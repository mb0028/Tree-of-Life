using UnityEngine;

public class ChangeMaterialFloat : MonoBehaviour
{
    public bool playOnStart = false;
    public Material material;
    public string propertyToChange = "";
    public float startVal = 1;
    public float endVal = 0;
    public float duration = 1;

    void Start()
    {
        if (playOnStart)
        {
            _ChangeVal();
        }
    }

    public void _ChangeVal()
    {
        material.SetFloat(propertyToChange, startVal);
    }

}
